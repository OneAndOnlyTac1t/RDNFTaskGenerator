namespace TDNFGenerator.Model
{
    public class SingleTask : ITask
    {
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string Text { get; set; }

        public SingleTask()
        {
        }
    }
}
