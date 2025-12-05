namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    public class Reports : CommandModule<CommandContext> //TODO don't use litterbox api and send directly through discord
    {
        [Command("reports")]
        public static async Task<string> reports()
        {
            string filePath = "reports.txt";

            if (!File.Exists(filePath))
            {
                return "The reports file does not exist / no reports have been made yet";
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
                            return $"Логи репортов сохранены тут: {responseBody}";
                        }
                        else
                        {
                            return $"Failed to upload reports file to Litterbox. Status code: {response.StatusCode}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return $"An error occurred while uploading the reports file: {ex.Message}";
            }
        }
    }
}