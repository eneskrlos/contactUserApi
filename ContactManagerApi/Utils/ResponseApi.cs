namespace ContactManagerApi.Utils
{
    public class ResponseApi
    {
        public string code { get; set; }

        public string message { get; set; }

        public ResponseApi()
        {
            code = "";
            message = "";
        }

        public ResponseApi(string code, string message)
        {
            this.code = code;
            this.message = message;
        }


        public string responsetoString() {
            return "Respondio con un" + code + " mensjae: " + message;
        }
    }
}
