(function (jQuery) {
    "use strict";
    jQuery(document).ready(function () {
        jQuery.getScript('/js/signalr/dist/browser/signalr.js', function () {
            jQuery(document).ready(function () {
                const connection = new signalR.HubConnectionBuilder().withUrl("/signalRHub").build();

                connection.on("ReceiveMessage", receiveMessageHandler);

                connection.start().then(() => {
                    const username = $("#username").val();
                    console.log("Connected by", username);
                    //connection.invoke("SetCustomUserID", username)
                    //    .then(() => {
                    //        console.log("Custom user ID set successfully");
                    //    })
                    //    .catch((err) => {
                    //        console.log("Error setting custom user ID: " + err);
                    //    });

                }).catch((err) => {
                    console.log(err);
                })
            });
        });
        
    });

})(jQuery);

function receiveMessageHandler(args) {
    console.log(args)
    alert("Receive Message Handler")
}
