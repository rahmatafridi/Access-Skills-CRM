"use strict";

// Class definition
var KTAvatarDemo = function () {
	// Private functions
    var initDemos = function () {
        var avatar1 = new KTAvatar('kt_user_avatar_1');
    };

	return {
		// public functions
		init: function() {
			initDemos();
		}
	};
}();

$(document).ready(function () {
    KTAvatarDemo.init();
});

