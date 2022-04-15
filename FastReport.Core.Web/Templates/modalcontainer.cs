﻿using FastReport.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using FastReport.Web.Application.Localizations;
using FastReport.Web.Controllers;

namespace FastReport.Web
{
    partial class WebReport
    {
        private const string template_pscustom =
@"      PageSelector = '&PageRange=PageNumbers&PageNumbers='+ PageSelectorInput[0].value;
        OnFirst[0].classList.remove('active');
        OnAll[0].classList.remove('active');";

        private const string template_modalcontainerscript =
@"    var but = document.querySelectorAll('.fr-webreport-settings-btn');
       var modalOverlay = document.querySelector('.modalcontainer-overlay');
       var modalBtnsSubmit = document.querySelectorAll('.fr-webreport-popup-content-btn-submit');
       var activebtn = document.querySelectorAll('.fr-webreport-popup-content-export-parameters-button');          
       var modals = document.querySelectorAll('.modalcontainer');

            modalBtnsSubmit.forEach((el) => {
        el.addEventListener('click', (e) => {
            modalOverlay.classList.remove('modalcontainer-overlay--visible');
	        modals.forEach((el) => {
		        el.classList.remove('modalcontainer--visible');
	        });
        });
        });

        but.forEach((el) => {
        el.addEventListener('click', (e) => {
        let path = e.currentTarget.getAttribute('data-path');
       
        modals.forEach((el) => {
            el.classList.remove('modalcontainer--visible');
        });
            });
        });
  
        modalOverlay.addEventListener('click', (e) => {
          
        if (e.target == modalOverlay || e.target == modalBtnsSubmit) {
	        modalOverlay.classList.remove('modalcontainer-overlay--visible');
	        modals.forEach((el) => {
                el.innerHTML = '';
		        el.classList.remove('modalcontainer--visible');
	        });
          }
        });

        activebtn.forEach((el) => {
            el.addEventListener('click', (e) => {
                {
                    el.classList.toggle('active');
                }
            });
        });

        var PageSelector = '&PageRange=All';
        var OnAll = document.getElementsByName('OnAllClick');
        var OnFirst = document.getElementsByName('OnFirstClick');
        var PageSelectorInput = document.getElementsByName('PageSelectorInput');

        function OnFirstClick() {
            for (var i = 0; i < PageSelectorInput.length; i++) {
                PageSelectorInput[i].value = CurrentPage.value;
            }
            for (var i = 0; i < OnAll.length; i++) {
                OnAll[i].classList.remove('active');
            }
            PageSelectorInput[0].value = CurrentPage.value;
            OnAll[0].classList.remove('active');
            PageSelector = '&PageRange=PageNumbers&PageNumbers=' + CurrentPage.value;
        }

        function OnAllClick() {
            PageSelectorInput[0].value = '1 - ' + AllPages.value;
            OnFirst[0].classList.remove('active');
            PageSelector = '&PageRange=All';
        }
        var PSFirst = '';
        var PSLast = ''
        var CustomPagesArray";

        
        string template_modalcontainer()
        {
            var templateModalContainer = $@"
            <div class=""modalcontainers"">
                <div class=""modalcontainer-overlay"">
                    <script>
                        setTimeout(function () {{
                            {template_FR}.getExportSettings();
                        }}, 100);  
                    </script>
                    <div class = ""content-modalcontainer""></div>
                </div>
            </div>";
            return templateModalContainer;
        }
    }

}
