using Microsoft.Practices.ServiceLocation;
using Queue.Terminal.Enums;
using Queue.Terminal.Views;
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
            Start();
        }

        public void SetNavigationService(NavigationService service)
        {
            navigationService = service;
        }

        public void SetCurrentPage(PageType? page)
        {
            if (page == PageType.SelectService)
            {
                userRequest.Reset();
            }

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
                        SetCurrentPage(GetPageForModelState(userRequest.GetCurrentState()));
                        break;

                    case PageType.SelectRequestType:
                        SetCurrentPage(GetPageForModelState(userRequest.GetCurrentState()));
                        break;

                    case PageType.SelectRequestDate:
                        SetCurrentPage(GetPageForModelState(userRequest.GetCurrentState()));
                        break;

                    case PageType.SetUsername:
                        SetCurrentPage(GetPageForModelState(userRequest.GetCurrentState()));
                        break;

                    case PageType.SelectSubjectsCount:
                        SetCurrentPage(GetPageForModelState(userRequest.GetCurrentState()));
                        break;

                    case PageType.PrintCoupon:
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
                        //SetCurrentPage(prevPage ?? PageType.SelectService);
                        break;

                    case PageType.SelectRequestDate:
                        //SetCurrentPage(prevPage ?? PageType.SelectService);
                        break;

                    case PageType.SetUsername:
                        //SetCurrentPage(prevPage ?? PageType.SelectService);
                        break;

                    case PageType.SelectSubjectsCount:
                        //SetCurrentPage(prevPage ?? PageType.SelectService);
                        break;

                    default:
                        Reset();
                        break;
                }
            }
        }

        private PageType GetPageForModelState(ClientRequestModelState state)
        {
            switch (state)
            {
                case ClientRequestModelState.SetService:
                    return PageType.SelectService;

                case ClientRequestModelState.SetRequestType:
                    return PageType.SelectRequestType;

                case ClientRequestModelState.SetRequestDate:
                    return PageType.SelectRequestDate;

                case ClientRequestModelState.SetClient:
                    return PageType.SetUsername;

                case ClientRequestModelState.SetSubjectsCount:
                    return PageType.SelectSubjectsCount;

                case ClientRequestModelState.Completed:
                    return PageType.PrintCoupon;

                default:
                    return PageType.SelectService;
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

                case PageType.SelectRequestDate:
                    return typeof(SelectRequestDatePage);

                case PageType.SetUsername:
                    return typeof(SetUsernamePage);

                case PageType.SelectSubjectsCount:
                    return typeof(SelectSubjectsCountPage);

                case PageType.SearchService:
                    return typeof(SearchServicePage);

                case PageType.PrintCoupon:
                    return typeof(PrintCouponPage);

                default:
                    throw new ApplicationException("Неизвестная страница: " + pageType.ToString());
            }
        }
    }
}