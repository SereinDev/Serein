
(function () {
    var getOnline = function (hook, _) {
        hook.doneEach(function () {
            if (document.querySelector("#online-count")) {
                var xhr = new XMLHttpRequest();
                xhr.open("get", "http://count.ongsat.com/api/online/queryCount?uri=127469ef347447698dd74c449881b877", true);
                xhr.send();
                xhr.onreadystatechange = () => {
                    console.log(xhr.responseText);
                    let returnJson = JSON.parse(xhr.responseText);
                    document.querySelector("#online-count").innerHTML = returnJson.data.count;
                };
            }
        });
    };

    $docsify = $docsify || {};
    $docsify.plugins = [].concat(getOnline, $docsify.plugins || []);

})();