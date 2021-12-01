using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class CommentManagementController : Controller
    {
        public IActionResult view_comment()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllComments = context.getAllComments();
            return View();
        }

        public string load_comment()
        {
            string output = "";
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            var allComments = context.getAllComments();
            foreach (var item in allComments)
            {
                output += "<tr>";
                output += $"<td>{item.GetType().GetProperty("CreatedAt").GetValue(item, null)}</td>";
                output += $"<td>{item.GetType().GetProperty("CommentContent").GetValue(item, null)}<input type='text' class='CommentId' value='{item.GetType().GetProperty("CommentId").GetValue(item, null) }' hidden></td>";
                output += $"<td>{item.GetType().GetProperty("UserName").GetValue(item, null)}</td>";
                output += $"<td>{item.GetType().GetProperty("ProductName").GetValue(item, null)}</td><td style='text-align:center;'>";

                if ((int)item.GetType().GetProperty("Reply").GetValue(item, null) == 1)
                    output += "<input class='form-check-input' type='checkbox' value='' checked>";
                else
                    output += "<input class='form-check-input' type='checkbox' value=''></td>";
                output += @$"<td>
                            <div class='form-button-action'>
                                <button type = 'button' data-toggle='tooltip' title='' class='btn btn-link btn-danger' data-original-title='Trả lời bình luận'>
                                 <a href = 'javascript:void(0)' onclick='return window.open()' class='active' ui-toggle-class=''>
                                 <i class='fa fa-reply' aria-hidden='true'></i></a>
                            </button>"; // Chỗ này chưa code onclick='return window.open()'
                //System.Diagnostics.Debug.WriteLine((int)item.GetType().GetProperty("CommentStatus").GetValue(item, null));
                if ((int)item.GetType().GetProperty("CommentStatus").GetValue(item, null) == 1)
                {
                    output += @$"
                                <button type = 'button' data-toggle='tooltip' title='' class='btn btn-link btn-primary' data-original-title='Hiển thị bình luận'>
                                 <i class='fa-thumb-styling fa fa-eye' aria-hidden='true'></i>
                            </button>";
                    output += @$"
                                <button type = 'button' data-toggle='tooltip' title='' class='btn btn-link btn-danger' data-original-title='Ẩn bình luận' hidden>
                                 <i class='fa-thumb-styling fa fa-eye-slash' aria-hidden='true'></i>
                            </button>";
                }
                else
                {
                    output += @$"
                                <button type = 'button' data-toggle='tooltip' title='' class='btn btn-link btn-primary' data-original-title='Hiển thị bình luận' hidden>
                                 <i class='fa-thumb-styling fa fa-eye' aria-hidden='true'></i>
                            </button>";
                    output += @$"
                                <button type = 'button' data-toggle='tooltip' title='' class='btn btn-link btn-danger' data-original-title='Ẩn bình luận'>
                                 <i class='fa-thumb-styling fa fa-eye-slash' aria-hidden='true'></i>
                            </button>";
                }
                output += @"<button type='button' data-toggle='tooltip' title='' class='btn btn-link btn-danger' data-original-title='Xóa bình luận'>
                               <a href = 'javascript:void(0)' class= 'active' ui-toggle-class= ''>
                                    <i class= 'fa fa-times text-danger text'></i>
                                </a>
                            </button></div></td></tr>";
            }
            return output;
        }
    }
}
