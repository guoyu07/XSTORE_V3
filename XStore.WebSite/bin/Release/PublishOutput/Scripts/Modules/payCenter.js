
$("#zfb_submitBtn").css("display", "none");
$("input[type=radio]").on("change", function () {
    console.log($(this).attr("id"))
    if ($(this).attr("id") === "zfbPay") {
        $("#zfb_submitBtn").css("display", "block");
        $("#wx_submit_order").css("display", "none");
    }
    if ($(this).attr("id") === "wxPay") {
        $("#wx_submit_order").css("display", "block");
        $("#zfb_submitBtn").css("display", "none");
    }
})
$(".submit").on("click", "#zfb_submitBtn", function () {
   // alert('zfb_submitBtn')
})
$(".submit").on("click", "#wx_submit_order", function () {
  //  alert('wx_submit_order')
  // callpay();
})