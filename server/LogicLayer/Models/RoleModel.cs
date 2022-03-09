using LogicLayer.Models.Interfaces;

namespace LogicLayer.Models
{
    public class RoleModel : IAccountModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
