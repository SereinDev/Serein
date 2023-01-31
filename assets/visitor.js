(function () {
    var visitor = function (hook, vm) {
        hook.afterEach(function (html) {
            return `<div id="${window.location.pathname}${window.location.hash.replace(/\?.+?$/, '')}" class="leancloud_visitors">
            阅读量&nbsp;·&nbsp;<span class="leancloud-visitors-count">?</span>
        </div>` + html;
        });
    };

    $docsify = $docsify || {};
    $docsify.plugins = [].concat(visitor, $docsify.plugins || []);
})();