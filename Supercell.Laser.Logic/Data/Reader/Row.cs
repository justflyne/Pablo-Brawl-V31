using System;
using System.Collections.Generic;
using System.Reflection;
using Supercell.Laser.Logic.Data.Helper;

namespace Supercell.Laser.Logic.Data.Reader
{
    public class Row
    {
        private readonly Table _table;
        public readonly int RowStart;

        public Row(Table table)
        {
            _table = table;
            RowStart = _table.GetColumnRowCount();
            _table.AddRow(this);
        }

        public int Offset => RowStart;

        public void LoadData(LogicData data)
        {
            foreach (var property in data.GetType().GetProperties(BindingFlags.Instance |
                                                                  BindingFlags.NonPublic |
                                                                  BindingFlags.Public))
            {
                if (property.CanRead && property.CanWrite)
                {
                    if (property.PropertyType.IsArray)
                    {
                        var elementType = property.PropertyType.GetElementType();
                        if (elementType == typeof(byte))
                            property.SetValue(data, LoadBoolArray(property.Name));
                        else if (elementType == typeof(int))
                            property.SetValue(data, LoadIntArray(property.Name));
                        else if (elementType == typeof(string))
                            property.SetValue(data, LoadStringArray(property.Name));
                    }
                    else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var genericArguments = property.PropertyType.GetGenericArguments();
                        var concreteListType = typeof(List<>).MakeGenericType(genericArguments);
                        var newList = Activator.CreateInstance(concreteListType);
                        var addMethod = concreteListType.GetMethod("Add");

                        for (var i = Offset; i < Offset + GetArraySize(property.Name); i++)
                        {
                            var value = GetValue(property.Name, i - Offset);

                            // Восстановление значения из предыдущего элемента, если значение пустое
                            if (string.IsNullOrEmpty(value) && i != Offset)
                            {
                                var previousValue = GetValue(property.Name, i - Offset - 1);
                                value = previousValue; // использовать значение предшествующего элемента
                            }

                            try
                            {
                                if (string.IsNullOrEmpty(value))
                                {
                                    // Если значение пустое, добавляем значение по умолчанию для типа
                                    var defaultValue = genericArguments[0].IsValueType ? Activator.CreateInstance(genericArguments[0]) : null;
                                    addMethod.Invoke(newList, new[] { defaultValue });
                                }
                                else
                                {
                                    var convertedValue = Convert.ChangeType(value.Trim(), genericArguments[0]);
                                    addMethod.Invoke(newList, new[] { convertedValue });
                                }
                            }
                            catch (Exception ex)
                            {
                                //Console.WriteLine($"Ошибка при добавлении значения '{value}' в список: {ex.Message}");
                            }
                        }

                        property.SetValue(data, newList);
                    }
                    else if (property.PropertyType == typeof(LogicData) || property.PropertyType.BaseType == typeof(LogicData))
                    {
                        var pData = (LogicData)Activator.CreateInstance(property.PropertyType);
                        LoadData(pData);
                        property.SetValue(data, pData);
                    }
                    else
                    {
                        var value = GetValue(property.Name, 0);

                        if (!string.IsNullOrEmpty(value))
                        {
                            try
                            {
                                // Логируем значение перед преобразованием
                                //Console.WriteLine($"Синхронизируем '{value}' для свойства '{property.Name}' в тип '{property.PropertyType.Name}'");

                                // Используем Convert.ChangeType с учетом типа данных
                                var convertedValue = Convert.ChangeType(value.Trim(), property.PropertyType);
                                property.SetValue(data, convertedValue);
                            }
                            catch (Exception ex)
                            {
                                //Console.WriteLine($"Ошибка при установке значения для свойства {property.Name}: {ex.Message}");
                            }
                        }
                    }
                }
            }
        }

        public int GetArraySize(string name)
        {
            var index = _table.GetColumnIndexByName(name);
            if (index == -1)
            {
                //Console.WriteLine($"Ошибка: столбец с именем '{name}' не найден.");
                return 0; // или выбросьте исключение
            }
            var size = _table.GetArraySizeAt(this, index);
            if (size < 0)
            {
                //Console.WriteLine($"Предупреждение: Неверный размер массива для '{name}'.");
                return 0; // или выбросьте исключение
            }
            return size;
        }

        public string GetName()
        {
            return _table.GetValueAt(0, RowStart);
        }

        public string GetValue(string name, int level)
        {
            // Проверяем переданные параметры, чтобы гарантировать, что они в пределах допустимого диапазона
            int index = level + RowStart;
            if (index < 0 || index >= _table.GetRowCount()) // Используйте правильный метод для получения общего количества строк
            {
                //Console.WriteLine($"Ошибка: уровень {level} выходит за пределы допустимого диапазона.");
                return string.Empty; // или выбросьте исключение
            }
            return _table.GetValue(name, index);
        }

        private bool[] LoadBoolArray(string column)
        {
            var arraySize = GetArraySize(column);
            var array = new bool[arraySize];

            for (var i = 0; i < arraySize; i++)
            {
                var value = GetValue(column, i);
                if (bool.TryParse(value, out var boolean))
                {
                    array[i] = boolean;
                }
            }

            return array;
        }

        private int[] LoadIntArray(string column)
        {
            var arraySize = GetArraySize(column);
            var array = new int[arraySize];

            for (var i = 0; i < arraySize; i++)
            {
                var value = GetValue(column, i);
                if (int.TryParse(value.Trim(), out var number))
                {
                    array[i] = number;
                }
                else
                {
                    //Console.WriteLine($"Не удалось преобразовать '{value}' в int. Индекс: {i}");
                }
            }

            return array;
        }

        private string[] LoadStringArray(string column)
        {
            var arraySize = GetArraySize(column);
            var array = new string[arraySize];

            for (var i = 0; i < arraySize; i++)
            {
                array[i] = GetValue(column, i);
            }

            return array;
        }
    }
}