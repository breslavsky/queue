namespace Queue.Model
{
    public abstract class ServiceSchedule : Schedule
    {
        #region properties

        public virtual Service Service { get; set; }

        #endregion properties
    }
}