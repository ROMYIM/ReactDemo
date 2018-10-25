namespace ReactDemo.Domain.Models.Conference
{
    public class ConferenceMember
    {
        public int MemberId { get; private set; }

        public ConferenceMember(int memberId) => MemberId = memberId;
       
    }
}