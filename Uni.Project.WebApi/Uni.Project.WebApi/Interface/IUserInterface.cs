using Uni.Project.WebApi.Domain;

namespace Uni.Project.WebApi.Interface
{
    public interface IUserInterface
    {
        public string CadUser(UserDomain user);
        public string Login(string email, string password);
        public string Download(string id);
    }
}
