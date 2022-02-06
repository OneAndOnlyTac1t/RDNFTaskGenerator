namespace TDNFGenerator.Model
{
    public class SingleTask
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public static string Text { get; private set; }

        public SingleTask(string text)
        {
            Text = text;
        }
    }
}
