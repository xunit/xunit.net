(function () {
    function hide(elements) {
        for (let i = 0; i < elements.length; i++) {
            elements.item(i).classList.add("hidden");
        }
    }

    function show(elements) {
        for (let i = 0; i < elements.length; i++) {
            elements.item(i).classList.remove("hidden");
        }
    }

    var $stable = document.getElementsByClassName("version-stable"),
        $ci = document.getElementsByClassName("version-ci"),
        $versionPickers = document.getElementsByClassName("version-picker");

    for (let i = 0; i < $versionPickers.length; i++) {
        $versionPickers.item(i).addEventListener('change', function(e) {
            var target = this.dataset.selector;

            if (target === ".version-stable")
                show($stable);
            else
                hide($stable);

            if (target === ".version-ci")
                show($ci);
            else
                hide($ci);
        });
    };
})();
