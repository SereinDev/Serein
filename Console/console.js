line=0;
function AppendText(str){
    ConsoleDiv=document.querySelector("#console");
    line=line+1;
    if (str=="#clear"){
        Clear();
    }
    else{
        var div=document.createElement("div");
        div.innerHTML=str;
        ConsoleDiv.appendChild(div);
    }
    if(line>250){
        ConsoleDiv.removeChild(ConsoleDiv.children[0]);
        line=line-1;
    }
    ConsoleDiv.scrollTop=ConsoleDiv.scrollHeight;
}
function Clear(){
    document.querySelector("#console").innerHTML="";
    line=0;
}