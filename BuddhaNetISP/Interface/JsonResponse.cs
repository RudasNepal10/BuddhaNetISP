namespace BuddhaNetISP.Interface
{
    public class JsonResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object ResponseData { get; set; }
        public string User { get; set; }

    }
}
