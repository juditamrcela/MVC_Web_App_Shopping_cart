$(function () {
    if ($("a.confirmDeletion").length) {
        $("a.confirmDeletion").click(() => {
            if (!confirm("Confim deletion")) return false;
        });
    }
    if ($("div.alert.notification").length) { //ovo provjeravamo je li postoji
        setTimeout(() => {
            $("div.alert.notification").fadeOut();
        }, 20);
    }
});

function readURL(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();
        reader.onload = function (e) {
            $("img#imgpreview").attr("source", e.target.result).width(200).height(200);

        };
        reader.readAsDefaultURL(input.files[0]);

    }
}