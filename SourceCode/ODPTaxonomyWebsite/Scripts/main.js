
//(function(window, document, undefined){

//    util = {};
//    
//    util.save = function(){
//        var formArray = $("form").serializeArray();
//        console.log("saved ..."+ JSON.stringify(formArray));
//    };

//    util.reset = function () {

//        alertify.set({
//            labels: {
//                ok: "OK",
//                cancel: "Cancel"
//            },
//            delay: 5000,
//            buttonReverse: true,
//            buttonFocus: "none"
//        });
//    };


//    $("#confirm").on('click', function () {
//        util.reset();
//        alertify.confirm("Please confirm this", function (e) {
//            if (e) {
//                //window.location.href = "revise.html";
//                util.save();
//            } else {
//                alertify.error("You've clicked Cancel");
//            }
//        });
//        return false;
//    });

//})(window, document);




$(document).ready(function(){
    
    $("form").submit(function(evt){
        evt.preventDefault();
    });







});

<!--Alertify-->


//$(document).ready(function(){
//    $('input').each(function(){
//        var self = $(this),
//            label = self.next(),
//            label_text = label.text();

//        label.remove();
//        self.iCheck({
//            checkboxClass: 'icheckbox_line-tax',
//            radioClass: 'iradio_line-tax',
//            insert: '<div class="icheck_line-icon"></div>' + label_text
//        });
//    });
//});


//Scroll for anchors

    //$(function () {
    //$('a[href*=#]:not([href=#])').click(function () {
    //    if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
    //        var target = $(this.hash);
    //        target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
    //        if (target.length) {
    //            $('html,body').animate({
    //scrollTop: target.offset().top
    //}, 1000);
    //            return false;
    //}
    //}
    //});
    //});



(function($){
    $.fn.serializeObject = function(){

        var self = this,
            json = {},
            push_counters = {},
            patterns = {
                "validate": /^[a-zA-Z][a-zA-Z0-9_]*(?:\[(?:\d*|[a-zA-Z0-9_]+)\])*$/,
                "key":      /[a-zA-Z0-9_]+|(?=\[\])/g,
                "push":     /^$/,
                "fixed":    /^\d+$/,
                "named":    /^[a-zA-Z0-9_]+$/
};


        this.build = function(base, key, value){
            base[key] = value;
            return base;
};

        this.push_counter = function(key){
            if(push_counters[key] === undefined){
                push_counters[key] = 0;
}
            return push_counters[key]++;
};

        $.each($(this).serializeArray(), function(){

    // skip invalid keys
            if(!patterns.validate.test(this.name)){
                return;
}

            var k,
                keys = this.name.match(patterns.key),
                merge = this.value,
                reverse_key = this.name;

            while((k = keys.pop()) !== undefined){

    // adjust reverse_key
                reverse_key = reverse_key.replace(new RegExp("\\[" + k + "\\]$"), '');

    // push
                if(k.match(patterns.push)){
                    merge = self.build([], self.push_counter(reverse_key), merge);
}

    // fixed
else if(k.match(patterns.fixed)){
                    merge = self.build([], k, merge);
}

    // named
else if(k.match(patterns.named)){
                    merge = self.build({}, k, merge);
}
}

            json = $.extend(true, json, merge);
});

        return json;
};
})(jQuery);