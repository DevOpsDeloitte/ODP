
app = angular.module("formApp", ["firebase", "ngSanitize"]);

//http://stackoverflow.com/questions/15449325/how-can-i-preserve-new-lines-in-an-angular-partial

app.filter("nl2br", function ($filter) {
    return function (data) {
        if (!data) return data;
        return data.replace(/\n\r?/g, '<br />');
    };
});

app.filter('newline', function ($sce) {
    return function (text) {
        if (!text) return text;
        text = text.replace(/\n/g, '<br />');
        return $sce.trustAsHtml(text);
    }
});

