using Firebase.Database;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;

namespace IucMarket.Service
{
    public abstract class BaseService
    {
        public Firebase.Auth.FirebaseAuthProvider FirebaseAuthProvider { get; private set; }
        public FirebaseClient FirebaseClient { get; private set; }
        public FirebaseAdmin.Auth.FirebaseAuth FirebaseAuthAdmin { get; private set; }

        public BaseService()
        {
            FirebaseClient = new FirebaseClient("https://iuc-market-default-rtdb.firebaseio.com/");
            FirebaseAuthProvider = new Firebase.Auth.FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig("AIzaSyB5XNkrCBWQucwLAmY_1dtx1-fwjyVnfjg"));
            FirebaseOptions config = new FirebaseOptions
            (
                "service_account",
                "iuc-market",
                "955d4ea4cae470e597798eaab8f24f2b2e41c889",
                "-----BEGIN PRIVATE KEY-----\nMIIEvwIBADANBgkqhkiG9w0BAQEFAASCBKkwggSlAgEAAoIBAQDn2Wpt7st0g5uW\ngPGlPhZIQBpkb2t5OL6BoPwkeiXOVV0rnbt5nd1e2G3LBYD3q5WzonuFIsz5YFuf\ng7BB50SjJwcQG4EeePuFFk/ksNqidVYGVorJyMadpUcJOwwP6k5EvQwk/YQupBbY\n7JR/ZGJnQjXUB/GbzWmUXtKw12m660UMV8unmFJVAhKGkdBAtB8Me+clZDOdfI0X\nwL7qqvYXYIIeVZ6z/TTZcmzbd99KmAuixPw9oP2D6RJst6pDM+NYHLZ05ZsnIiRt\nzhXvPUNoMdMgdJ4KhYyD8W7Lfow2p8gCPU5D/Hw4NioHLDd5F4tn5SW87+626zNu\nSm4gSPMrAgMBAAECggEAbUNfrnZSyNK7MOcMuqCzdJJNPdbqkeLlmQaqvXAZQA+n\nzIXV099Lc8bQm1S5Lj9dWh5xUtGPQtkf5OF1X+GjTbZ3VCZ6J8fBVTuqJC8aomRk\nybDgUG8/9wxsmVOMADYec7OaNRaKxEZNhCaeZxkCbQJdhtLFkPTx1FwJtBCYwd73\nxFHc7iSeO6nZknrxKtEaigtXq1x1f6kHQu7p2bijEt6kSt3lkDm+HRVoklf7i1x7\nqJunj6hvGUjIHxEhUg5BU+IgnQ/y/2Rryq1MgG3nVQeKh/vl3xl6iWQui3OvNRok\nASBd8+S/PeR4JXb9XhBqTRaLcZ603U0Qy6PxtJ/LGQKBgQD973pKheCSxTgxdcMK\n1bhQ/FCJSEuJyCVI5a6eySU/MEwZ8zYdaQYD2DD15CQwTI11cXiLimWpqqadj3g/\naNHxhzlKLJLNt/055H/Q0bvMLBF6iSMxPS6tvIpvl2PLfaW5l6rf7VLIsXy/Ows5\n+rPVmdFso/Fc4VOrleDfZfWOkwKBgQDpu/gyL6aQD6Hq4HBh8o0Ok622cF5X5haD\nAN0Vepeyzcj3VaeknkB5FJ891ZVOU7Ezj4i9LVtwnNtZqoxFXa/kbbkVnVCmrGgQ\nu+HsS8g+qTol8WIpOoovLy/tbFde6fVFKBssPyAjGEnPTnNdT3X2hNGe3NNddDeT\ncZOscCNQCQKBgQCdk2mzrPf6m9+O8aCirJS/zJK9XdtiGIzqe3ysk+1FFNdkkwPV\npEJTSGi5bWT3g8mUQ2GmVa9YZckWpNzdnFILKvpmCNrgRXgh4KVgE4YR4JbIDymD\neI/qx8CwFqWatNGOJYungUGJwHnEwQLqa6QEvHq0i9dO55RgOZVdi4uheQKBgQCg\nubtAd3foFHfZVaCyGeJZpb+MdtTJSNUuHIfq3zQ1pkCNo/71ukTHfiDKmnZjODXg\nKNF0lR4N4C8OB7MReA0d3T2q2VS+aEvfRfNjU/FM9X2g3c4MX87qB17Duv6Rq/wX\nJC8bTAVDXM5UmbPG9H8/l7G1tlA6MVKON0m/CBWgUQKBgQCCsxyXbAEYlvQ8DOjy\nkXUPAgiEUOr9WOvTPqVXgdzyu9b4LYA6er6S7NqUM4oqQczb05FRO74bXXzjVcjy\naJgoWUby3V5JRJnxHwHm52qTnV3XoS26k6VCPyQ+yo6D2T8wPSlKcNAWAKS9g9at\nVB2E3uPwFPCgG22eeCKmvqWvhg==\n-----END PRIVATE KEY-----\n",
                "firebase-adminsdk-2wp9j@iuc-market.iam.gserviceaccount.com",
                "101423117111183978751",
                "https://accounts.google.com/o/oauth2/auth",
                "https://oauth2.googleapis.com/token",
                "https://www.googleapis.com/oauth2/v1/certs",
                "https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-2wp9j%40iuc-market.iam.gserviceaccount.com"
            );
            if (FirebaseApp.DefaultInstance == null)
                FirebaseApp.Create
                (
                    new AppOptions
                    {
                        Credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(config))
                    }
                );

            FirebaseAuthAdmin = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
        }
    }

    public class FirebaseOptions
    {
        public string type { get; set; }
        public string project_id { get; set; }
        public string private_key_id { get; set; }
        public string private_key { get; set; }
        public string client_email { get; set; }
        public string client_id { get; set; }
        public string auth_uri { get; set; }
        public string token_uri { get; set; }
        public string auth_provider_x509_cert_url { get; set; }
        public string client_x509_cert_url { get; set; }

        public FirebaseOptions()
        {

        }

        public FirebaseOptions(string type, string project_id, string private_key_id, string private_key, string client_email, string client_id, string auth_uri, string token_uri,
            string auth_provider_x509_cert_url, string client_x509_cert_url)
        {
            this.type = type;
            this.project_id = project_id;
            this.private_key_id = private_key_id;
            this.private_key = private_key;
            this.client_email = client_email;
            this.client_id = client_id;
            this.auth_uri = auth_uri;
            this.token_uri = token_uri;
            this.auth_provider_x509_cert_url = auth_provider_x509_cert_url;
            this.client_x509_cert_url = client_x509_cert_url;
        }
    }
}