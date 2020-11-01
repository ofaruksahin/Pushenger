using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pushenger.Core.Interfaces;
using System;

namespace Pushenger.Api.Hubs
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseHub : Hub
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// 
        /// </summary>
        public HttpContext HttpContext => Context.GetHttpContext();

        /// <summary>
        /// 
        /// </summary>
        public string ConnectionId => Context.ConnectionId;
        /// <summary>
        /// 
        /// </summary>
        public IQueryCollection Query => HttpContext.Request.Query;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_unitOfWork"></param>
        public BaseHub(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
    }
}
