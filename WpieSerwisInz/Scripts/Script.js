
function SlidePanel(item) {
    for (i = 0; i < 30; i++) {
        if (i != item.id) {
            var hd = 'panel' + i;
            $('div#' + hd).hide("normal");
        }
    }
    var name = 'panel' + item.id;
    $('div#' + name).slideToggle("normal");
}


function DownloadFile(idc) {
    var actionUrl = '@Url.Action("Download", id)';

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        url: "/JunctionXml/Download",
        dataType: 'json',
        type: 'POST',
        data: JSON.stringify({
            id: idc
        }),
        success: 
            $.fileDownload(result)
    });

}

function generateComplete(result) {
    if (result.Success == true) {
        // this could/should already be set in the HTML
        formGenerate.action = "/JunctionXml/Download";
        formGenerate.target = iframeFile;

        hidData = result.Data;
        formGenerate.submit();
    } else
    {
    // TODO: display error messages
    }
}