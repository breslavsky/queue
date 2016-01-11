using AutoMapper;
using NHibernate.Criterion;
using Queue.Model;
using Queue.Model.Common;
using Queue.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    public partial class ServerService
    {
        public async Task<DTO.DefaultConfig> GetDefaultConfig()
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<DefaultConfig>(ConfigType.Default);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    return Mapper.Map<DefaultConfig, DTO.DefaultConfig>(сonfig);
                }
            });
        }

        public async Task<DTO.DefaultConfig> EditDefaultConfig(DTO.DefaultConfig source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Config);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<DefaultConfig>(ConfigType.Default);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    сonfig.QueueName = source.QueueName;
                    сonfig.WorkStartTime = source.WorkStartTime;
                    сonfig.WorkFinishTime = source.WorkFinishTime;
                    сonfig.MaxClientRequests = source.MaxClientRequests;
                    сonfig.MaxRenderingTime = source.MaxRenderingTime;
                    сonfig.IsDebug = source.IsDebug;

                    var errors = сonfig.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(сonfig);
                    transaction.Commit();

                    QueueInstance.ConfigUpdated(сonfig);

                    return Mapper.Map<DefaultConfig, DTO.DefaultConfig>(сonfig);
                }
            });
        }

        public async Task<DTO.DesignConfig> GetDesignConfig()
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<DesignConfig>(ConfigType.Design);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    return Mapper.Map<DesignConfig, DTO.DesignConfig>(сonfig);
                }
            });
        }

        public async Task<DTO.DesignConfig> EditDesignConfig(DTO.DesignConfig source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Config);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<DesignConfig>(ConfigType.Design);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    сonfig.LogoSmall = source.LogoSmall;

                    var errors = сonfig.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(сonfig);
                    transaction.Commit();

                    QueueInstance.ConfigUpdated(сonfig);

                    return Mapper.Map<DesignConfig, DTO.DesignConfig>(сonfig);
                }
            });
        }

        public async Task<DTO.CouponConfig> GetCouponConfig()
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<CouponConfig>(ConfigType.Coupon);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    return Mapper.Map<CouponConfig, DTO.CouponConfig>(сonfig);
                }
            });
        }

        public async Task<DTO.CouponConfig> EditCouponConfig(DTO.CouponConfig source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Config);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<CouponConfig>(ConfigType.Coupon);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    сonfig.Sections = source.Sections;

                    session.Save(сonfig);
                    transaction.Commit();

                    QueueInstance.ConfigUpdated(сonfig);

                    return Mapper.Map<CouponConfig, DTO.CouponConfig>(сonfig);
                }
            });
        }

        public async Task<DTO.SMTPConfig> GetSMTPConfig()
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Config);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<SMTPConfig>(ConfigType.SMTP);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    return Mapper.Map<SMTPConfig, DTO.SMTPConfig>(сonfig);
                }
            });
        }

        public async Task<DTO.SMTPConfig> EditSMTPConfig(DTO.SMTPConfig source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Config);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<SMTPConfig>(ConfigType.SMTP);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    сonfig.Server = source.Server;
                    сonfig.Port = source.Port;
                    сonfig.EnableSsl = source.EnableSsl;
                    сonfig.User = source.User;
                    сonfig.Password = source.Password;
                    сonfig.From = source.From;

                    var errors = сonfig.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(сonfig);
                    transaction.Commit();

                    QueueInstance.ConfigUpdated(сonfig);

                    return Mapper.Map<SMTPConfig, DTO.SMTPConfig>(сonfig);
                }
            });
        }

        public async Task<DTO.PortalConfig> GetPortalConfig()
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Config);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<PortalConfig>(ConfigType.Portal);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    return Mapper.Map<PortalConfig, DTO.PortalConfig>(сonfig);
                }
            });
        }

        public async Task<DTO.PortalConfig> EditPortalConfig(DTO.PortalConfig source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Config);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<PortalConfig>(ConfigType.Portal);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    сonfig.CurrentDayRecording = source.CurrentDayRecording;

                    var errors = сonfig.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(сonfig);
                    transaction.Commit();

                    QueueInstance.ConfigUpdated(сonfig);

                    return Mapper.Map<PortalConfig, DTO.PortalConfig>(сonfig);
                }
            });
        }

        public async Task<DTO.MediaConfig> GetMediaConfig()
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<MediaConfig>(ConfigType.Media);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    return Mapper.Map<MediaConfig, DTO.MediaConfig>(сonfig);
                }
            });
        }

        public async Task<DTO.MediaConfig> EditMediaConfig(DTO.MediaConfig source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Config);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<MediaConfig>(ConfigType.Media);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    сonfig.ServiceUrl = source.ServiceUrl;
                    сonfig.Ticker = source.Ticker;
                    сonfig.TickerSpeed = source.TickerSpeed;

                    var errors = сonfig.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(сonfig);
                    transaction.Commit();

                    QueueInstance.ConfigUpdated(сonfig);

                    return Mapper.Map<MediaConfig, DTO.MediaConfig>(сonfig);
                }
            });
        }

        public async Task<DTO.MediaConfigFile[]> GetMediaConfigFiles()
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var files = session.CreateCriteria<MediaConfigFile>()
                        .AddOrder(Order.Asc("Name"))
                        .List<MediaConfigFile>();

                    return Mapper.Map<IList<MediaConfigFile>, DTO.MediaConfigFile[]>(files);
                }
            });
        }

        public async Task<DTO.MediaConfigFile> GetMediaConfigFile(Guid mediaConfigFileId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Config);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var mediaConfigFile = session.Get<MediaConfigFile>(mediaConfigFileId);
                    if (mediaConfigFile == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(mediaConfigFileId), string.Format("Медиафайл [{0}] не найден", mediaConfigFileId));
                    }

                    return Mapper.Map<MediaConfigFile, DTO.MediaConfigFile>(mediaConfigFile);
                }
            });
        }

        public async Task<DTO.MediaConfigFile> EditMediaConfigFile(DTO.MediaConfigFile source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Config);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    MediaConfigFile mediaConfigFile;

                    if (!source.Empty())
                    {
                        var mediaConfigFileId = source.Id;
                        mediaConfigFile = session.Get<MediaConfigFile>(mediaConfigFileId);
                        if (mediaConfigFile == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(mediaConfigFileId), string.Format("Медиафайл [{0}] не найден", mediaConfigFileId));
                        }
                    }
                    else
                    {
                        mediaConfigFile = new MediaConfigFile();
                    }

                    mediaConfigFile.Name = source.Name;

                    var errors = mediaConfigFile.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(mediaConfigFile);
                    transaction.Commit();

                    return Mapper.Map<MediaConfigFile, DTO.MediaConfigFile>(mediaConfigFile);
                }
            });
        }

        public async Task DeleteMediaConfigFile(Guid mediaConfigFileId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Config);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var mediaConfigFile = session.Get<MediaConfigFile>(mediaConfigFileId);
                    if (mediaConfigFile == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(mediaConfigFileId), string.Format("Медиафайл [{0}] не найден", mediaConfigFileId));
                    }

                    session.Delete(mediaConfigFile);
                    transaction.Commit();
                }
            });
        }

        public async Task<DTO.TerminalConfig> GetTerminalConfig()
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<TerminalConfig>(ConfigType.Terminal);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    return Mapper.Map<TerminalConfig, DTO.TerminalConfig>(сonfig);
                }
            });
        }

        public async Task<DTO.TerminalConfig> EditTerminalConfig(DTO.TerminalConfig source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Config);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<TerminalConfig>(ConfigType.Terminal);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    сonfig.PIN = source.PIN;
                    сonfig.CurrentDayRecording = source.CurrentDayRecording;
                    сonfig.Columns = source.Columns;
                    сonfig.Rows = source.Rows;
                    сonfig.Pages = source.Pages;
                    сonfig.StartPage = source.StartPage;

                    var errors = сonfig.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(сonfig);
                    transaction.Commit();

                    QueueInstance.ConfigUpdated(сonfig);

                    return Mapper.Map<TerminalConfig, DTO.TerminalConfig>(сonfig);
                }
            });
        }

        public async Task<DTO.NotificationConfig> GetNotificationConfig()
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<NotificationConfig>(ConfigType.Notification);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    return Mapper.Map<NotificationConfig, DTO.NotificationConfig>(сonfig);
                }
            });
        }

        public async Task<DTO.NotificationConfig> EditNotificationConfig(DTO.NotificationConfig source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Config);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var сonfig = session.Get<NotificationConfig>(ConfigType.Notification);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    сonfig.ClientRequestsLength = source.ClientRequestsLength;

                    var errors = сonfig.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(сonfig);
                    transaction.Commit();

                    QueueInstance.ConfigUpdated(сonfig);

                    return Mapper.Map<NotificationConfig, DTO.NotificationConfig>(сonfig);
                }
            });
        }
    }
}