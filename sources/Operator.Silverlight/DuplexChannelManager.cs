using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Queue.Operator.Silverlight
{
    public class Channel<T> : IDisposable
    {
        private IClientChannel channel;

        public Channel(IClientChannel channel)
        {
            this.channel = channel;
        }

        public bool IsConnected
        {
            get { return channel.State == CommunicationState.Opened; }
        }

        public T Service
        {
            get { return (T)channel; }
        }

        public void Dispose()
        {
            try
            {
                switch (channel.State)
                {
                    case CommunicationState.Opened:
                        channel.Close();
                        break;

                    case CommunicationState.Faulted:
                        channel.Abort();
                        break;
                }

                channel.Dispose();
            }
            catch (Exception)
            {
                // no more
            }
        }
    }

    public class DuplexChannelBuilder<T>
    {
        private DuplexChannelFactory<T> channelFactory;

        public DuplexChannelBuilder(object callbackInstance, Binding binding, EndpointAddress endpoint)
        {
            channelFactory = new DuplexChannelFactory<T>(new InstanceContext(callbackInstance), binding, endpoint);
        }

        public Channel<T> CreateChannel()
        {
            return new Channel<T>((IClientChannel)channelFactory.CreateChannel());
        }

        public Channel<T> CreateChannel(object callbackObject)
        {
            return new Channel<T>((IClientChannel)channelFactory.CreateChannel(new InstanceContext(callbackObject)));
        }
    }
}