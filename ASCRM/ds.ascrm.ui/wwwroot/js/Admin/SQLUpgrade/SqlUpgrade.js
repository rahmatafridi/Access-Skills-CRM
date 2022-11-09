
var sqlCollection = [];
function UpgradeSQLUpgrade()
{
    $.ajax({
        url: "/api/SQLUpgradeApi/InsertProductVersion",
        //data: JSON.stringify(data),
        type: "Post",
        contentType: "application/json",
        dataType: "json"
    }).done(function (response)
    {
        if (response === true)
        {
            toastr.success('SQL scripts have been upgraded successfully.', 'Success!');
            getUpgradeHtml(sqlCollection, true, false,'');
            
        } else {
            toastr.success('SQL scripts have not been upgraded. Please try again.', 'Error!');
        }
    }).fail(function (xhr) {
        toastr.error(xhr.responseText, 'Error!');
    });
}

function GetProductVersion()
{
    $.ajax({
        url: "/api/SQLUpgradeApi/GetProductVersions",
        type: "GET",
        contentType: "application/json",
        dataType: "json"
    }).done(function (response) {
        sqlCollection = [];
        sqlCollection = response;
        getUpgradeHtml(response,false,false,'');
    }).fail(function (xhr) {
        toastr.error(xhr.responseText, 'Error!');
    });
}

function DeleteSQLFile(fileName) {
    $.ajax({
        url: "/api/SQLUpgradeApi/DeleteSqlFile",
        data: {deletedFileName: fileName },
        type: "GET",
        contentType: "application/json",
        dataType: "json"
    }).done(function (response) {
        if (response === true)
        {
            toastr.success('SQL scripts have been deleted successfully.', 'Success!');
            getUpgradeHtml(sqlCollection, false, true, fileName);

        } else {
            toastr.success('SQL scripts have not been deleted. Please try again.', 'Error!');
        }
    }).fail(function (xhr) {
        toastr.error(xhr.responseText, 'Error!');
    });
}

function getUpgradeHtml(response, isUpgraded, isDeleted,fileName)
{
    var _version = '';
    var _no = 1;
    for (var count = 0; count < response.length; count++)
    {
        if (isUpgraded === true)
        {
            _version += '<li class="list-group-item d-flex justify-content-between"><p class="p-0 m-0 flex-grow-1">' + _no++ + '.   ' + response[count].SQLScriptVersion + '</p><button class="button button5">Upgrade</button><i title="Delete" class="fas fa-times-circle" onclick=DeleteSQLFile("' + encodeURI(response[count].SQLScriptVersion) + '") style="font-size:25px;color:#B22222;cursor: pointer;"></i></li>';
        }
        else if (isDeleted === true)
        {
            if (fileName !== response[count].SQLScriptVersion)
            {
                _version += '<li class="list-group-item d-flex justify-content-between"><p class="p-0 m-0 flex-grow-1">' + _no++ + '.   ' + response[count].SQLScriptVersion + '</p><i title="Delete" class="fas fa-times-circle" onclick=DeleteSQLFile("' + encodeURI(response[count].SQLScriptVersion) + '") style="font-size:25px;color:#B22222;cursor: pointer;"></i></li>';
            }
        }
        else
        {
            _version += '<li class="list-group-item d-flex justify-content-between"><p class="p-0 m-0 flex-grow-1">' + _no++ + '.   ' + response[count].SQLScriptVersion + '</p><i title="Delete" class="fas fa-times-circle" onclick=DeleteSQLFile("' + encodeURI(response[count].SQLScriptVersion) + '") style="font-size:25px;color:#B22222;cursor: pointer;"></i></li>';
        }
    }
    $("#sqlVersion").html(_version);
}


