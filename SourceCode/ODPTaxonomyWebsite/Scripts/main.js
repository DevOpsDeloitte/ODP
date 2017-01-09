




$(document).ready(function(){
    
    $("form").submit(function(evt){
        evt.preventDefault();
    });

    $('.category-toggle').click(function (e) {
        e.preventDefault();

        var $this = $(this);
        var $id = $this.attr("id");
        //console.log($id);
        if ($this.parent().find(".opener-"+$id).hasClass('hide-b')) {
            $this.parent().find(".opener-"+$id).removeClass('hide-b');
            //$this.parent().find(".opener-"+$id).slideDown(350);
        }
        else {
            $this.parent().find(".opener-"+$id).addClass('hide-b');
            //$this.parent().find(".opener-"+$id).slideUp(350);
        }
        
    });







});




//Scroll for anchors

    $(function () {
        $('a[href*=#]:not([href=#])').click(function () {
            if(this.hash == '#IQS' || this.hash == '#ODP'){
                //console.log(this.hash);
                return;
            }
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                $('html,body').animate({
                        scrollTop: target.offset().top
                        }, 1000);
                return false;
    }
    }
    });
    });



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