line=0
function AppendText(str){
    line=line+1;
    if (str=="#clear"){
        Clear();
    }
    else{
        $("#console").append("<div>"+str+"</div>");

    }
    if(line>250){
        $("#console > *:first-child").remove();
        line=line-1;
    }
}
function Clear(){
    $("#console").html("");
    line=0;
}