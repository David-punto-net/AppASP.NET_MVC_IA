using AppVentasWeb.Common;
using AppVentasWeb.Data;
using AppVentasWeb.Data.Entidades;
using AppVentasWeb.DTOs;
using AppVentasWeb.Helper;
using AppVentasWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Transbank.Common;
using Transbank.Webpay.Common;
using Transbank.Webpay.WebpayPlus;


namespace AppVentasWeb.Controllers;

public class WebPayController : Controller
{

    private readonly Transaction _tx;
    private readonly IUserHelper _userHelper;
    private readonly DataContex _context;
    private readonly IOrdersHelper _ordersHelper;

    public WebPayController(DataContex context, IUserHelper userHelper, IOrdersHelper ordersHelper)
    {
        _context = context;
        _userHelper = userHelper;
        _ordersHelper = ordersHelper;

        _tx = new Transaction(new Transbank.Common.Options(IntegrationCommerceCodes.WEBPAY_PLUS, IntegrationApiKeys.WEBPAY, WebpayIntegrationType.Test));

    }

    [Authorize]
    public async Task<IActionResult> Index()
    {

        User user = await _userHelper.GetUserAsync(User.Identity.Name);
        if (user == null)
        {
            return NotFound();
        }

        if (!TempData.ContainsKey("MontoTotal"))
        {
            return RedirectToAction("ShowCart", "Home");
        }

        var amount = decimal.Parse(TempData["MontoTotal"].ToString());

        if (amount <= 0)
        {
            return RedirectToAction("ShowCart", "Home");
        }

        var request = new TransbankRequestDTO
        {
            Buy_order = Guid.NewGuid().ToString("N")[..10],
            Session_id = user.Id.ToString(),
            Return_url = Url.Action("Commit", "WebPay", null, Request.Scheme)!,
            Amount = amount
        };

        if (request == null ||
              string.IsNullOrWhiteSpace(request.Buy_order) ||
              string.IsNullOrWhiteSpace(request.Session_id) ||
              string.IsNullOrWhiteSpace(request.Return_url) ||
              request.Amount <= 0)
        {
            //return NotFound();
            return RedirectToAction("ShowCart", "Home");
        }

        try
        {
            var crearTransaccion = _tx.Create(request.Buy_order, request.Session_id, request.Amount, request.Return_url);

            WebPayResponseViewModel model = new()
            {
                Url = crearTransaccion.Url,
                Token = crearTransaccion.Token,
                UrlCompleta = crearTransaccion.Url + "?token_ws=" + crearTransaccion.Token
            };

            ViewBag.tokenWs = model.Token;
            ViewBag.Amount= request.Amount;
            ViewBag.BuyOrder = request.Buy_order;
            ViewBag.Session_id = request.Session_id;

            return Redirect(model.UrlCompleta);

            //return View(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }

    }

    public async Task<IActionResult> Commit(string token_ws)
    {

        User user = await _userHelper.GetUserAsync(User.Identity.Name);
        if (user == null)
        {
            return NotFound();
        }

        try
        {
            var confirmarTransaccion = _tx.Commit(token_ws);

            var model = new WebPayCommitViewModel
            {
                Vci = confirmarTransaccion.Vci,
                Amount = (decimal)confirmarTransaccion.Amount,
                Status = confirmarTransaccion.Status,
                BuyOrder = confirmarTransaccion.BuyOrder,
                SessionId = confirmarTransaccion.SessionId,
                TokenWebpay = token_ws,
                //CardDetail = confirmarTransaccion.CardDetail,
                AccountingDate = confirmarTransaccion.AccountingDate,
                TransactionDate = (DateTime)confirmarTransaccion.TransactionDate,
                AuthorizationCode = confirmarTransaccion.AuthorizationCode,
                //PaymentTypeCode = confirmarTransaccion.PaymentTypeCode,
                //ResponseCode = (int)confirmarTransaccion.ResponseCode,
                //InstallmentsAmount = (decimal)confirmarTransaccion.InstallmentsAmount,
                //InstallmentsNumber = (int)confirmarTransaccion.InstallmentsNumber,
                //Balance = (decimal)confirmarTransaccion.Balance
            };

            if (confirmarTransaccion.ResponseCode == 0)
            {
                var modelcard = new ShowCartViewModel();

                modelcard.User = user;
                modelcard.TemporalSales = await _context.TemporalSales
                                                .Include(ts => ts.Producto)
                                                .ThenInclude(p => p.ProductImages)
                                                .Where(ts => ts.User.Id == user.Id)
                                                .ToListAsync();

                if (modelcard.WebpayViewModels == null)
                {
                    modelcard.WebpayViewModels = new List<WebPayCommitViewModel>();
                }
                modelcard.WebpayViewModels.Add(model);

                Response response = await _ordersHelper.ProcessOrderAsync(modelcard);

            }

            return View(model);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("ShowCart", "Home");
        }

    }


}
