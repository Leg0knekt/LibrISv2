using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrISv2
{
    public class PageControl
    {
        private static PageAddIssue pAddIssue;
        private static PageAdminMenu pAdminMenu;
        private static PageAuthorization pAuth;
        private static PageCatalog pCatalog;
        private static PageConnectionProperties pConnectionProperties;
        private static PageDebtors pDebtors;
        private static PageEmployeeMenu pEmployeeMenu;
        private static PageIssuance pIssuance;
        private static PageNewClient pNewClient;
        private static PageNewEmployee pNewEmployee;
        private static PageReservationView pReservationView;
        public static PageAddIssue PageAddIssue
        {
            get
            {
                if (pAddIssue == null) { pAddIssue = new PageAddIssue(); }
                return pAddIssue;
            }
        }
        public static PageAdminMenu PageAdminMenu
        {
            get
            {
                if (pAdminMenu == null)
                {
                    pAdminMenu = new PageAdminMenu();
                }
                return pAdminMenu;
            }
        }
        public static PageAuthorization PageAuth
        {
            get
            {
                if (pAuth == null) { pAuth = new PageAuthorization(); }
                return pAuth;
            }
        }
        public static PageCatalog PageCatalog
        {
            get
            {
                if (pCatalog == null) { pCatalog = new PageCatalog(); }
                return pCatalog;
            }
        }
        public static PageConnectionProperties PageConnectionProperties
        {
            get
            {
                if (pConnectionProperties == null)
                {
                    pConnectionProperties = new PageConnectionProperties();
                }
                return pConnectionProperties;
            }
        }
        public static PageDebtors PageDebtors
        {
            get
            {
                if (pDebtors == null) { pDebtors = new PageDebtors(); }
                return pDebtors;
            }
        }
        public static PageEmployeeMenu PageEmployeeMenu
        {
            get
            {
                if (pEmployeeMenu == null) { pEmployeeMenu = new PageEmployeeMenu(); }
                return pEmployeeMenu;
            }
        }
        public static PageIssuance PageIssuance
        {
            get
            {
                if (pIssuance == null) { pIssuance = new PageIssuance(); }
                return pIssuance;
            }
        }
        public static PageNewClient PageNewClient
        {
            get
            {
                if (pNewClient == null) { pNewClient = new PageNewClient(); }
                return pNewClient;
            }
        }
        public static PageNewEmployee PageNewEmployee
        {
            get
            {
                if (pNewEmployee == null) { pNewEmployee = new PageNewEmployee(); }
                return pNewEmployee;
            }
        }
        public static PageReservationView PageReservationView
        {
            get
            {
                if (pReservationView == null) { pReservationView = new PageReservationView(); }
                return pReservationView;
            }
        }
    }
}
