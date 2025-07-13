namespace Client.Infrastructure
{
    public static class ApiUrl
    {
        public static class Auth
        {
            public static string Login(string baseUri) => $"{baseUri}/auth/login";
            public static string ChangePassword(string baseUri) => $"{baseUri}/auth/change-pass";
            public static string Recovery(string baseUri) => $"{baseUri}/auth/recovery";
            public static string CompleteRecovery(string baseUri) => $"{baseUri}/auth/complete-recovery";
        }

        public static class Inventario
        {
            public static string Get(string baseUri) => $"{baseUri}/inventario/getall";
            public static string GetItem(string baseUri, int Id) => $"{baseUri}/inventario/get/{Id}";
            public static string AddItem(string baseUri) => $"{baseUri}/inventario/new";
            public static string UpdateItem(string baseUri) => $"{baseUri}/inventario/upd";
            public static string DeleteItem(string baseUri) => $"{baseUri}/inventario/del";
            public static string Count(string baseUri) => $"{baseUri}/inventario/count";
        }

        public static class Categoria
        {
            public static string Get(string baseUri) => $"{baseUri}/categoria/getall";
            public static string GetItem(string baseUri, int Id) => $"{baseUri}/categoria/get/{Id}";
            public static string AddItem(string baseUri) => $"{baseUri}/categoria/new";
            public static string UpdateItem(string baseUri) => $"{baseUri}/categoria/upd";
            public static string DeleteItem(string baseUri) => $"{baseUri}/categoria/del";
            public static string Count(string baseUri) => $"{baseUri}/categoria/count";
        }

        public static class Tamano
        {
            public static string Get(string baseUri) => $"{baseUri}/tamano/getall";
            public static string GetItem(string baseUri, int Id) => $"{baseUri}/tamano/get/{Id}";
            public static string AddItem(string baseUri) => $"{baseUri}/tamano/new";
            public static string UpdateItem(string baseUri) => $"{baseUri}/tamano/upd";
            public static string DeleteItem(string baseUri) => $"{baseUri}/tamano/del";
            public static string Count(string baseUri) => $"{baseUri}/tamano/count";
        }

    }
}
