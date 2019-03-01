using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactDemo.Domain.Models.Party
{

    [Table("party_role")]
    public class Role : Entity
    {
        private string _name;
        public string Name
        {
            get 
            { 
                if (_name == null)
                {
                    throw new NullReferenceException();
                }
                return _name;
            }

            set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("the name of party role can not be empty");
                }
                _name = value;
            }
        }

        private int _organizationID;
        [ForeignKey("organization_id")]
        public int OrganizationID
        {
            private set => _organizationID = value;

            get
            {
                if (_organizationID == 0)
                {
                    throw new NullReferenceException("organization id can not be zero");
                }
                return _organizationID;
            }
        }

        public Role(int organizationID, string name)
        {
            _organizationID = organizationID;
            Name = name;
        }
    }
}