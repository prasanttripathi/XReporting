
$(document).ready(function () {

    $('.del').live('click', function () {
        $(this).parent().parent().remove();
    });

    $('.add').live('click', function () {
        var areaname = $(this).closest('tr').find('input').val();

        if (areaname != null && areaname != "" && areaname != undefined) {
            $(this).val('Delete');
            $(this).attr('class', 'del');
            var appendTxt = "<tr><td><input type='text' size='50' name='input_area' /></td> <td><input type='text' size='50' name='input_area_desc' /></td> <td><input type='button' style='width:80px' class='add' value='Add More' /> </td></tr>";
            $("#options-table tr:last").after(appendTxt);
        }
    });

    $('.delType').live('click', function () {
        $(this).parent().parent().remove();
    });

    $('.addType').live('click', function () {
        var typename = $(this).closest('tr').find('input').val();
        var chkFilled = checkValidation($(this).closest('tr'));
        if (typename != null && typename != "" && typename != undefined && chkFilled == true) {
            $(this).val('Delete');
            $(this).attr('class', 'delType');
            var rowcount = $('#TypeDetails')[0].rows.length;
            var appendTxt = "<tr  runat='server'><td><input type='text' size='50' name='input_type' /></td> "
                            + "<td> <input type='checkbox' name='input_step_one'  id='input_step_one_" + rowcount + "' runat='server' checked='checked' disabled='disabled' value='1' />Initiate &nbsp;"
                          + " <input type='checkbox' name='input_step_two' id='input_step_two_" + rowcount + "' runat='server' checked='checked' disabled='disabled' value='2'  />Approval &nbsp;"
                          + " <input type='checkbox' name='input_step_three' id='input_step_three_" + rowcount + "' runat='server'  value='3' />Review &nbsp;"
                          + " <input type='checkbox' name='input_step_four' id='input_step_four_" + rowcount + "' runat='server' checked='checked' disabled='disabled' value='4'  />Response &nbsp;"
                          + " <input type='checkbox' name='input_step_five' id='input_step_five_" + rowcount + "' runat='server'  value='5'  />Close Out &nbsp; </td> <td><input type='button' style='width:80px' class='addType' value='Add More' /></td></tr>";
            $("#TypeDetails tr:last").after(appendTxt);
        }
    });


});

function isPositiveInteger(s) {
    return !!s.match(/^[a-zA-Z0-9]+$/);
    // or Rob W suggests
    // return /^\d+$/.test(s);
}



function checkValidation(row) {
    var cells = row[0].cells;
    var cell = cells[1];
    var flag = false;
    for (j = 0; j < cell.children.length; j++) {

        var inputElem = cell.children[j];

        if (inputElem.getAttribute('type') == "checkbox") {
            if (inputElem.checked) {
                flag = true;
                return flag;
            }
        }

    }
    return flag;
}

var data = [];
function gatherData() {
    data = [];
    var table = $('#TypeDetails')[0];
    for (r = 2; r < table.rows.length; r++) {

        var row = table.rows[r];
        var cells = row.cells;
        var value = "";
        for (c = 0; c < cells.length; c++) {

            var cell = cells[c];
            var sequence = "";
            for (j = 0; j < cell.children.length; j++) {

                var inputElem = cell.children[j];

                if (inputElem.getAttribute('type') == "text" || inputElem.getAttribute('type') == "checkbox") {
                    if (inputElem.getAttribute('type') == "text") {
                        if (inputElem.value != "" && inputElem.value != undefined && inputElem.value != null)
                            value += inputElem.value + ";";
                    }

                    if (inputElem.getAttribute('type') == "checkbox") {
                       
                            if (inputElem.checked) {
                                sequence = sequence + "." + inputElem.value;  //TO DO. Add "." in end also

                            }
                            if (j == cell.children.length - 1) {

                                if (value != "" && value != undefined && value != null)
                                {
                                    value = value + sequence +".";
                                    var rowData = {};
                                    rowData.inputValue = value;
                                    data.push(rowData);
                                }
                            }
                       
                    }
                }

            }

        }
    }
}



function startExec() {

    //var exceutionflag = FormValidation();
    var exceutionflag = $("#checkTerms")[0].checked;
    if (exceutionflag) {
        gatherData();
        var typeValues = "";
        for (i = 0; i < data.length; i++) {
            typeValues += data[i].inputValue + "#";
        }
        typeValues = typeValues.substring(0, typeValues.length - 1);
        $("#TypeInfoHidden").val(typeValues);
        return true;
    }
    else
    {
        alert("Please confirm details!");
        return false;
    }

}

function ShowConfirmScreen() {
    var exceutionflag = FormValidation();
    if (exceutionflag) {
        disable("#mainTable");
        $("#confirmTable")[0].style.display = "block";
       // $("#SiteLinkHidden").val($("#SiteLinkHidden")[0].value + "/" + $("#txtSiteTitle")[0].value);
        $("#input_region")[0].innerHTML = $("#txtRegionName")[0].value;
        $("#input_site_link")[0].innerHTML = $("#SiteLinkHidden")[0].value+"/" + $("#txtSiteTitle")[0].value;
        $("#input_prefix")[0].innerHTML = $("#txtRegionPrefix")[0].value;

        var controls = document.getElementsByName("input_area");
        $("#input_areas")[0].innerHTML = "";
        var areaValues = "";
        for (var i = 0; i < controls.length; i++) {
            if (controls[i].value != "" && controls[i].value != undefined && controls[i].value != null)
            {
                $("#input_areas")[0].innerHTML += controls[i].value + "<br>";
                areaValues += controls[i].value + ";";
            }
        }
        areaValues = areaValues.substring(0, areaValues.length - 1);
        $("#AreaInfoHidden").val(areaValues);

        var desc_controls = document.getElementsByName("input_area_desc");
        var areaDescValues = "";
        for (var i = 0; i < desc_controls.length; i++) {
            if (controls[i].value != "" && controls[i].value != undefined && controls[i].value != null) {
                areaDescValues += desc_controls[i].value + ";";
            }
        }
        areaDescValues = areaDescValues.substring(0, areaDescValues.length - 1);
        $("#AreaDescriptionInfoHidden").val(areaDescValues);

        var region_info = $("#txtRegionName")[0].value + ";" + $("#txtSiteTitle")[0].value + ";" + $("#txtRegionPrefix")[0].value;
        $("#RegionInfoHidden").val(region_info);
        $("#RegionTitleHidden").val($("#txtSiteTitle")[0].value);
        $("#RegionNameHidden").val($("#txtRegionName")[0].value);
       
        gatherData();
        var typeValues = "";
        for (i = 0; i < data.length; i++) {
            typeValues += data[i].inputValue + "#";
        }
        typeValues = typeValues.substring(0, typeValues.length - 1);
        $("#TypeInfoHidden").val(typeValues);

        typeValues += "#";
        $("#input_types")[0].innerHTML = "";
        for (var j = 0; j < typeValues.split('#').length ; j++) {
            var type_first_part = typeValues.split('#')[j].split(';')[0];
            $("#input_types")[0].innerHTML += type_first_part + "<br>";
        }
        return false;
    }
    else {
        return false;
    }
}



function disable(table_id) {
    var inputs = $(table_id)[0].getElementsByTagName('input');
    for (var i = 0; i < inputs.length; ++i)
        inputs[i].disabled = true;
    //return true;
}

function enable(table_id) {
    var inputs = $(table_id)[0].getElementsByTagName('input');
    for (var i = 0; i < inputs.length; ++i)
    {
        if (inputs[i].name.toLowerCase().indexOf("input_step_one") >= 0 || inputs[i].name.toLowerCase().indexOf("input_step_two") >= 0 || inputs[i].name.toLowerCase().indexOf("input_step_four") >= 0
            || inputs[i].className=="disabledCtrl" )
        { }
        else {
            inputs[i].disabled = false;
        }
    }
        
}

function BackScreen() {
    $("#confirmTable")[0].style.display = "none";
    //$("#mainTable")[0].style.display = "block";
    enable("#mainTable");
}

function checkStepValidation(row) {
    var cells = row.cells;
    var cell = cells[1];
    var flag = false;
    for (j = 0; j < cell.children.length; j++) {

        var inputElem = cell.children[j];

        if (inputElem.getAttribute('type') == "checkbox") {
            if (inputElem.checked) {
                flag = true;
                return flag;
            }
        }

    }
    return flag;
}

function FormValidation() {

    var name = $("#txtRegionName")[0].value;
    if (name == "" || name == undefined || name == null) {
        alert("Please region name");
        return false;
    }
    else {
        if (name.length < 3)
        {
        alert("Region name must have atleast three characters");
        return false;
        }
    }

    var title = $("#txtSiteTitle")[0].value;
    if (title == "" || title == undefined || title == null) {
        alert("Please site title");
        return false;
    }
    else {
        if (title.length < 3) {
            alert("Site title must have atleast three characters");
            return false;
        }

        var title_Collection = $("#SiteTitleHidden")[0].value;       
        var index = title_Collection.toLowerCase().indexOf(title.toLowerCase());
        if (index > -1)
        {
            alert("Site title is already in use. Please enter new site title");
            return false;
        }
       
    }

    var prefix = $("#txtRegionPrefix")[0].value;
    if (prefix == "" || prefix == undefined || prefix == null) {
        alert("Please region prefix");
        return false;
    }
    else {
        if (prefix.length < 2) {
            alert("Prefix must have atleast two characters");
            return false;
        }
        var prefix_Collection = $("#PrefixHiddenField")[0].value;
        var index = prefix_Collection.toLowerCase().indexOf(prefix.toLowerCase());
        if (index > -1) {
            alert("Prefix is already in use. Please enter new prefix");
            return false;
        }
    }

    var area_input = $("input[name='input_area']");

    var type_input = $("input[name='input_type']");
    var regex = new RegExp("^[a-zA-Z0-9& -]+$");
    var isMinOneArea = false;

    for (var i = 0; i < area_input.length; i++) {
        if (area_input[i].value == "" || area_input[i].value == undefined || area_input[i].value == null) {
            
        }
        else
        {
            if (!regex.test(area_input[i].value))
            {
                area_input[i].focus();
                area_input[i].select();
                alert("Special characters are not allowed. Please enter only alphanumeric values.");
                return false;
            }
            isMinOneArea = true;
        }
    }
    
    if (!isMinOneArea) {
        alert("Please fill atleast one area");
        return false;
    }

   

    var isMinOneType = false;
    for (var i = 0; i < type_input.length; i++) {

        if (type_input[i].value == "" || type_input[i].value == undefined || type_input[i].value == null) {
           
        }
        else
        {           
            if (!regex.test(type_input[i].value)) {
                type_input[i].focus();
                type_input[i].select();
                alert("Special characters are not allowed. Please enter only alphanumeric values.");
                return false;
            }
            isMinOneType = true;
        }
    }

    if (!isMinOneType)
    {
        alert("Please fill atleast one type");
        return false;
    }

    var table = $('#TypeDetails')[0];
    var chkFlag = true;
    for (r = 2; r < table.rows.length; r++) {
        chkFlag = checkStepValidation(table.rows[r]);
        if (!chkFlag) {
            alert("Please select atleast one step for each type");
            return false;
        }

    }

    return true;

}


