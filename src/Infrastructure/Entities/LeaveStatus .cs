namespace Infrastructure.Entities
{
    public class LeaveStatus
    {
        public LeaveStatus()
        {
            this.Leaves = new HashSet<Leave>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Leave> Leaves { get; set; }
    }
}
