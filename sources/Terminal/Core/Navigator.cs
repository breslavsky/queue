using Microsoft.Practices.Unity;
using Queue.Terminal.Enums;
using Queue.Terminal.Views;
using System;
using System.Collections.Generic;
using System.Windows.Navigation;

namespace Queue.Terminal.Core
{
    public class Navigator
    {
        private static ClientRequestModelState[] Stages = new ClientRequestModelState[]
                                                                {
                                                                    ClientRequestModelState.SetService,
                                                                     ClientRequestModelState.SetRequestType,
                                                                     ClientRequestModelState.SetRequestDate,
                                                                     ClientRequestModelState.SetClient,
                                                                     ClientRequestModelState.SetSubjects,
                                                                     ClientRequestModelState.Completed
                                                                };

        private PageType? currentPage;
        private NavigationService navigationService;
        private List<ClientRequestModelState> history;

        [Dependency]
        public IUnityContainer UnityContainer { get; set; }

        [Dependency]
        public ClientRequestModel UserRequest { get; set; }

        public Navigator()
        {
            history = new List<ClientRequestModelState>();
        }

        public void Start()
        {
            currentPage = null;
            NextPage();
        }

        public void Reset()
        {
            ResetState();
            Start();
        }

        public void ResetState()
        {
            history.Clear();
            UserRequest.Reset();
        }

        public void SetNavigationService(NavigationService service)
        {
            navigationService = service;
        }

        public void SetCurrentPage(PageType? page)
        {
            if (page == PageType.SelectService)
            {
                UserRequest.Reset();
            }

            navigationService.Navigate(UnityContainer.Resolve(GetPageType(page.Value)));
            currentPage = page;
        }

        public void NextPage()
        {
            ClientRequestModelState state = UserRequest.GetCurrentState();
            CaptureState(state);

            if (!currentPage.HasValue)
            {
                SetCurrentPage(PageType.SelectService);
            }
            else
            {
                switch (currentPage.Value)
                {
                    case PageType.SearchService:
                        SetCurrentPage(GetPageForModelState(state));
                        break;

                    case PageType.SelectService:
                        SetCurrentPage(GetPageForModelState(state));
                        break;

                    case PageType.SelectRequestType:
                        SetCurrentPage(GetPageForModelState(state));
                        break;

                    case PageType.SelectRequestDate:
                        SetCurrentPage(GetPageForModelState(state));
                        break;

                    case PageType.SetUsername:
                        SetCurrentPage(GetPageForModelState(state));
                        break;

                    case PageType.SelectSubjects:
                        SetCurrentPage(GetPageForModelState(state));
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

        private void CaptureState(ClientRequestModelState state)
        {
            history.Add(state);
            if (!history.Contains(ClientRequestModelState.SetService))
            {
                history.Insert(0, ClientRequestModelState.SetService);
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
                ClientRequestModelState state = UserRequest.GetCurrentState();
                history.Remove(state);

                ClientRequestModelState prevState = history.Count > 0 ?
                                                        history[history.Count - 1] :
                                                        ClientRequestModelState.SetService;

                switch (currentPage)
                {
                    case PageType.SearchService:
                        Reset();
                        break;

                    case PageType.SelectService:
                        Reset();
                        break;

                    case PageType.SelectRequestType:
                        SetCurrentPage(GetPageForModelState(prevState));
                        break;

                    case PageType.SelectRequestDate:
                        SetCurrentPage(GetPageForModelState(prevState));
                        break;

                    case PageType.SetUsername:
                        SetCurrentPage(GetPageForModelState(prevState));
                        break;

                    case PageType.SelectSubjects:
                        SetCurrentPage(GetPageForModelState(prevState));
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

                case ClientRequestModelState.SetSubjects:
                    return PageType.SelectSubjects;

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
                    return typeof(SetClientPage);

                case PageType.SelectSubjects:
                    return typeof(SelectSubjectsPage);

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