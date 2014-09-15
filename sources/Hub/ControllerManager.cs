namespace Queue.Hub
{
    public class ControllerManager
    {
        private static class NestedControllerManager
        {
            internal static readonly ControllerManager controllerManager = new ControllerManager();
        }

        private static ControllerManager controllerManager
        {
            get { return NestedControllerManager.controllerManager; }
        }

        private Controller current;

        public static Controller Current
        {
            get { return controllerManager.current; }
        }

        private ControllerManager()
        {
            current = new Controller();
        }
    }
}