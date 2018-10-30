namespace ReactDemo.Domain.Models.Meeting
{
    public class ConferenceMember
    {
        public int MemberId { get; private set; }

        public ConferenceMember(int memberId) => MemberId = memberId;
       
    }
}