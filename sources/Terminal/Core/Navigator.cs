using Microsoft.Practices.ServiceLocation;
using Queue.Model.Common;
using Queue.Terminal.Enums;
using Queue.Terminal.Pages;
using System;
using System.Windows.Navigation;

namespace Queue.Terminal.Core
{
    public class Navigator
    {
        private ClientRequestModel userRequest;
        private PageType? currentPage;
        private NavigationService navigationService;

        public Navigator()
        {
            userRequest = ServiceLocator.Current.GetInstance<ClientRequestModel>();
        }

        public void Start()
        {
            currentPage = null;
            NextPage();
        }

        public void Reset()
        {
            userRequest.Reset();
            Start();
        }

        public void SetNavigationService(NavigationService service)
        {
            navigationService = service;
        }

        public void SetCurrentPage(PageType? page)
        {
            if (navigationService != null)
            {
                navigationService.Navigate(Activator.CreateInstance(GetPageType(page.Value)));
            }

            currentPage = page;
        }

        public void NextPage()
        {
            if (!currentPage.HasValue)
            {
                SetCurrentPage(PageType.SelectService);
            }
            else
            {
                switch (currentPage.Value)
                {
                    case PageType.SearchService:
                        SetCurrentPage(PageType.SelectRequestType);
                        break;

                    case PageType.SelectService:
                        SetCurrentPage(PageType.SelectRequestType);
                        break;

                    case PageType.SelectRequestType:
                        switch (userRequest.QueueType)
                        {
                            case ClientRequestType.Live:

                                SetCurrentPage(userRequest.SelectedService.ClientRequire ?
                                    PageType.SetUsername :
                                    PageType.SelectSubjectsCount);

                                break;

                            case ClientRequestType.Early:
                                SetCurrentPage(PageType.SelectDateTime);
                                break;
                        }
                        break;

                    case PageType.SelectDateTime:
                        SetCurrentPage(userRequest.SelectedService.ClientRequire ?
                                  PageType.SetUsername :
                                  PageType.SelectSubjectsCount);
                        break;

                    case PageType.SetUsername:
                        SetCurrentPage(PageType.SelectSubjectsCount);
                        break;

                    case PageType.SelectSubjectsCount:
                        Reset();
                        break;

                    default:
                        Reset();
                        break;
                }
            }
        }

        public void PrevPage()
        {
            if (!currentPage.HasValue)
            {
                SetCurrentPage(PageType.SelectService);
            }
            else
            {
                switch (currentPage)
                {
                    case PageType.SearchService:
                        Reset();
                        break;

                    case PageType.SelectService:
                        Reset();
                        break;

                    case PageType.SelectRequestType:
                        SetCurrentPage(PageType.SelectService);
                        break;

                    case PageType.SelectDateTime:
                        SetCurrentPage(PageType.SelectRequestType);
                        break;

                    case PageType.SetUsername:
                        SetCurrentPage(userRequest.QueueType == ClientRequestType.Live ?
                                                PageType.SelectRequestType :
                                                PageType.SelectDateTime);
                        break;

                    case PageType.SelectSubjectsCount:
                        SetCurrentPage(userRequest.SelectedService.ClientRequire ?
                                   PageType.SetUsername :
                                   PageType.SelectRequestType);
                        break;

                    default:
                        Reset();
                        break;
                }
            }
        }

        private Type GetPageType(PageType pageType)
        {
            switch (pageType)
            {
                case PageType.SelectService:
                    return typeof(SelectServicePage);

                case PageType.SelectRequestType:
                    return typeof(SelectRequestTypePage);

                case PageType.SelectDateTime:
                    return typeof(SelectDateTimeCalendarPage);

                case PageType.SetUsername:
                    return typeof(SetUsernamePage);

                case PageType.SelectSubjectsCount:
                    return typeof(SelectSubjectsCountPage);

                case PageType.SearchService:
                    return typeof(SearchServicePage);

                default:
                    throw new ApplicationException("Неизвестная страница: " + pageType.ToString());
            }
        }
    }
}