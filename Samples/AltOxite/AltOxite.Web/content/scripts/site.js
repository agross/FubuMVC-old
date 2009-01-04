window._emailRegex = /^[a-z0-9]+([-+\.]*[a-z0-9]+)*@[a-z0-9]+([-\.][a-z0-9]+)*$/i;

/** field hinting **/
$(document).ready(function() {
    $("form").hintify(window.stringResources);
});
$.extend(jQuery.expr[":"], {
    textarea: "$.nodeName(a, 'textarea')"
});
$.fn.extend({
    hintify: function(config) {
        if (!config)
            return this;

        this.submit(function() {
            $("[_hint]", this).each(function() { $(this).removeHint() });
        });

        $(window).unload(function() {
            $("form [_hint]").each(function() { $(this).removeHint() });
        });

        for (var key in config) {
            var keyParts = key.split(".");
            if (keyParts.length > 1) {
                $("#" + keyParts[0]).filter(":enabled").hint(config[key]);
            }
        }

        return this;
    },
    hint: function(hintText) {
        if (!!hintText && (this.is(":text") || this.is(":textarea"))) {
            this.attr("_hint", hintText);
            this.addHint()
            this.focus(function() { $(this).removeHint(); });
            this.blur(function() { $(this).addHint(); });
        }
        return this;
    },
    addHint: function() {
        if ($.trim(this.val()) === "") {
            this.addClass("hinted");
            this.removeClass("active");
            this.val(this.attr("_hint"));
        } else {
            this.addClass("active");
        }
    },
    removeHint: function() {
        if ($.trim(this.val()) === this.attr("_hint")) {
            this.val("");
            this.removeClass("hinted");
        }
        this.addClass("active");
    }
});

/*** gravatar fetch and alt change ***/
$(document).ready(function() {
    $('#comment_email').blur(function() {
        $('#comment_grav').changeGravatarTo($(this).val(), $('#comment_hashedEmail').val());
    });
    $('#comment_name').blur(function() {
        $('#comment_grav').changeGravatarAltTo($(this).val());
    });
});
$.fn.extend({
    changeGravatarTo: function(email, hashedEmail) {
        var gravatar = $(this).find("img.gravatar");

        var gravatarUrlParts = gravatar.attr("src").split("?");
        var gravatarPathParts = gravatarUrlParts[0].split("/");
        gravatarPathParts[gravatarPathParts.length - 1] = email.indexOf("@") > 0 && window._emailRegex.test(email) ? hex_md5(email) : hashedEmail;

        gravatar.attr("src", gravatarPathParts.join("/") + "?" + gravatarUrlParts[1]);
    },
    changeGravatarAltTo: function(name) {
        var gravatar = $(this).find("img.gravatar");
        if ($.trim(name) !== "") {
            gravatar.attr("title", name + " (gravatar)");
        } else {
            gravatar.attr("title", "(gravatar)");
        }
    }
});

/** username in the login form gets focus on load **/
$(document).ready(function() {
    $("#login_username").focus();
});

/** archives **/
$(document).ready(function() {
    $('.archives ul.yearList li.previous').each(function() {
        $(this).click(function() {
            $(this).toggleClass("open");
            $(this).find("h4>span").toggle();
            $(this).children("ul").toggle();
        });
        $(this).mouseover(function() {
            $(this).addClass("hover");
        });
        $(this).mouseout(function() {
            $(this).removeClass("hover");
        });
    });
});

/** flags **/
$(document).ready(function() {
    /* removal */
    $("form.remove.post").submit(function() {
        return window.confirm('really?');
    });
    $("form.flag.remove").submit(function() {
        var form = $(this);
        var flagged = $(this).parent(".flags").next(".flagged");
        flagged.fadeTo(350, .4);
        $.ajax({
            type: "POST",
            url: this.action,
            data: { commentId: this.commentId.value },
            success: function() { flagged.slideUp(250, function() { var comment = flagged.parent("li.comment"); comment.animate({ marginTop: -28 }, 300, "linear", function() { comment.remove(); comment = 0; }); flagged = 0; }); form.hide(1000); form = 0; },
            error: function() { flagged.fadeTo(350, 1); form = flagged = 0; }
        });
        return false;
    });
    /* approval */
    $("form.flag.approve").submit(function() {
        var form = $(this);
        var flagged = $(this).parent(".flags").next(".flagged");
        flagged.fadeTo(350, .4);
        $.ajax({
            type: "POST",
            url: this.action,
            data: { commentId: this.commentId.value },
            success: function() { flagged.fadeTo(350, 1); form = flagged = 0; },
            error: function() { flagged.fadeTo(350, .1); form = flagged = 0; }
        });
        return false;
    });
});
/** admins **/
$(document).ready(function() {
    if ($("#post_publishDate").is(":enabled")) {
        $("#post_publishDate").change(function() {
            $("#post_statePublished").attr("checked", "checked");
        });
        $("#post_statePublished").focus(function() {
            $("#post_publishDate").focus();
            if ($.trim($("#post_publishDate").val()) === "") {
                var date = new Date();
                $("#post_publishDate").val(date.toShortString());
            }
            $("#post_publishDate").blur();
        });
        $("#post_publishDate").datepicker({
            duration: "",
            dateFormat: "yy/mm/dd '8:00 AM'",
            showOn: "button",
            buttonImage: skinPath + "/images/calendar.png",
            buttonImageOnly: true,
            closeAtTop: false,
            isRTL: true
        });
    };

    $("input[@name='postState']").change(function() {
        if ($("#post_statePublished").is(":checked")) {
            $("#post_publishDate").addClass("active");
        } else {
            $("#post_publishDate").removeClass("active");
        }
    });

    $("#post_title").change(function() {
        $("#post_slug").slugify($(this).val());
    });

    $.fn.extend({
        slugify: function(string) {
            if (!this.is(":enabled"))
                return;

            slug = $.trim(string);

            if (slug && slug !== "") {
                var cleanReg = new RegExp("[^A-Za-z0-9-]", "g");
                var spaceReg = new RegExp("\\s+", "g");
                var dashReg = new RegExp("-+", "g");

                slug = slug.replace(spaceReg, '-');
                slug = slug.replace(dashReg, "-");
                slug = slug.replace(cleanReg, "");

                if (slug.length * 2 < string.length) {
                    return "";
                }

                if (slug.Length > 100) {
                    slug = slug.substring(0, 100);
                }
            }

            this.val(slug);
        }
    });
});

Date.prototype.toShortString = function() {
    var y = this.getYear();
    var year = y % 100;
    year += (year < 38) ? 2000 : 1900;
    return (this.getMonth() + 1).toString() + "/" + this.getDate() + "/" + year + " " + this.toLocaleTimeString();
};