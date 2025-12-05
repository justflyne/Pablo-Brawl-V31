namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    public class Battles : CommandModule<CommandContext> //TODO don't use litterbox api and send directly through discord
    {
        [Command("battles")]
        public static async Task<string> battles()
        {
            string filePath = "battles.txt";

            if (!File.Exists(filePath))
            {
                return "The battles file does not exist / no battles have been made yet";
            }

            try
            {
                using (HttpClient client = new())
                {
                    using (MultipartFormDataContent content = new())
                    {
                        byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
                        ByteArrayContent fileContent = new(fileBytes);
                        content.Add(fileContent, "fileToUpload", Path.GetFileName(filePath));

                        content.Add(new StringContent("fileupload"), "reqtype");
                        content.Add(new StringContent("72h"), "time");

                        // litterbox api
                        HttpResponseMessage response = await client.PostAsync(
                            "https://litterbox.catbox.moe/resources/internals/api.php",
                            content
                        );

                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            return $"Логи боев сохранены тут: {responseBody}";
                        }
                        else
                        {
                            return $"Failed to upload battles file to Litterbox. Status code: {response.StatusCode}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return $"An error occurred while uploading the battles file: {ex.Message}";
            }
        }
    }
}