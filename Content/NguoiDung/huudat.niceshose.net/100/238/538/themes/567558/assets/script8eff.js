// Biến khởi tạo
if ( typeof(formatMoney) == 'undefined' ){
	formatMoney = '';
}

var Nobita = {
	parseQueryString: function(){
		var str = window.location.search.toLowerCase();
		var objURL = {};
		str.replace(
			new RegExp("([^?=&]+)(=([^&]*))?", "g"),
			function($0, $1, $2, $3) {
				objURL[$1] = $3;
			});
		return objURL;
	},
	fixHeightProduct: function (data_parent, data_target, data_image) {
		var box_height = 0;
		var box_image = 0;
		var boxtarget = data_parent + ' ' + data_target;
		var boximg = data_parent + ' ' + data_target + ' ' + data_image;
		jQuery(boximg).css('height', 'auto');
		jQuery($(boxtarget)).css('height', 'auto');
		jQuery($(boxtarget)).removeClass('fixheight');
		jQuery($(boxtarget)).each(function() {
			if (jQuery(this).find($(data_image)).height() > box_image) {
				box_image = jQuery(this).find($(data_image)).height();
			}
		});
		if (box_image > 0) {
			jQuery(boximg).height(box_image);
		}
		jQuery($(boxtarget)).each(function() {
			if (jQuery(this).height() > box_height) {
				box_height = jQuery(this).height();
			}
		});
		jQuery($(boxtarget)).addClass('fixheight');
		if (box_height > 0) {
			jQuery($(boxtarget)).height(box_height);
		}
		try {
			fixheightcallback();
		} catch (ex) {}
	},
	clone_item_view: function(product) { 
		item_product = $('#clone-item .item');
		item_product.find('.title-review').html(product.name);
		item_product.find('.url-product').attr('href', product.url).attr('title',product.name);
		item_product.find('.price-review').html(product.price);
		if (product.compare_at_price != '' && product.hasSale) {
			item_product.find('.price-sale-review').html(product.compare_at_price).removeClass('hidden');
		} else {
			item_product.find('.price-sale-review').addClass('hidden').html('');
		}
		if (product.featured_image != '') {
			item_product.find('.box-image').find('img').attr('src',Bizweb.resizeImage(product.featured_image, 'small')).attr('alt',product.name);
		} else {
			item_product.find('.box-image').find('img').attr('src','//theme.hstatic.net/1000143413/1000246656/14/image-empty.jpg?v=79').attr('alt',product.name);
		}
		item_product.clone().removeClass('hidden').appendTo('#owl-demo-daxem > .product-item')
	},
	setCookiePopup: function (cvalue, exdays, nameCookie) {
		var d = new Date();
		d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
		$.cookie(nameCookie, cvalue, { expires: d, path: '/' });
	},
	getCookiePopup: function (nameCookie) {
		return $.cookie(nameCookie);
	},
	changeTemplate: function(element){
		// Change grid list in collection
		if ( $(element).hasClass('active') ) {

		} else {
			$('#event-grid > div:not(.clear-ajax)').hide();
			$('.box-product-lists > .loadmore, .box-product-lists > .pagination-default').hide();
			$('.icon-loading').show();
			$('.btn-change-list').removeClass('active');
			$(element).addClass('active');
			if( $(element).attr('data-template') == 'template-list' ) {
				$('#event-grid').addClass('template-list');
			} else {
				$('#event-grid').removeClass('template-list');
			}
		}
		jQuery('#event-grid').imagesLoaded(function() {
			$('.icon-loading').hide();
			$('#event-grid > div:not(.clear-ajax)').show();
			$('.box-product-lists > .loadmore, .box-product-lists > .pagination-default').show();
			jQuery(window).resize();
		});
	},
	filterItemInList: function(object){
		// Keyup find item in list filter collection
		q = object.val().toLowerCase();
		object.parent().next().find('li').show();
		if (q.length > 0) {
			object.parent().next().find('li').each(function() {
				if ($(this).find('label').attr("data-filter").indexOf(q) == -1)
					$(this).hide();
			})
		}
	},
	filterItemInList_2: function(object){
		// Keyup find item in list filter collection
		q = object.val().toLowerCase();
		q = slug_words(q).trimRight();
		object.parent().next().find('li').show();
		if (q.length > 0) {
			object.parent().next().find('li').each(function() {
				if (slug_words($(this).find('span').html()).indexOf(q) == -1)
					$(this).hide();
			})
		}
	},
	checkItemOwlShow: function(object,tab,a,b,c,d){
		// Check owl item next/prev show or hide
		if ( tab == 'tab' ) {
			item = object.find('.active').find('.owl-carousel');
		} else {
			item = object.find('.owl-carousel');
		}	
		if ( item.find('.owl-item.active').length < a && $(window).width() >= 1200 ) {
			item.find('.owl-controls').hide();
		}
		if ( item.find('.owl-item.active').length < b && $(window).width() >= 992 && $(window).width() < 1199 ) {
			item.find('.owl-controls').hide();
		}
		if ( item.find('.owl-item.active').length < c && $(window).width() >= 768 && $(window).width() < 991 ) {
			item.find('.owl-controls').hide();
		}
		if ( item.find('.owl-item.active').length < d && $(window).width() < 768 ) {
			item.find('.owl-controls').hide();
		}
	},
	buy_now: function(id){
		// Add a product in checkout
		var quantity = 1;
		var params = {
			type: 'POST',
			url: '/cart/add.js',
			data: 'quantity=' + quantity + '&VariantId=' + id,
			dataType: 'json',
			success: function(line_item) {
				window.location = '/checkout';
			},
			error: function(XMLHttpRequest, textStatus) {
				Bizweb.onError(XMLHttpRequest, textStatus);
			}
		};
		jQuery.ajax(params);
	},
	add_item: function(id){
		// Add a product in cart
		var quantity = 1;
		var params = {
			type: 'POST',
			url: '/cart/add.js',
			data: 'quantity=' + quantity + '&VariantId=' + id,
			dataType: 'json',
			success: function(line_item) {
				window.location = '/cart';
			},
			error: function(XMLHttpRequest, textStatus) {
				Bizweb.onError(XMLHttpRequest, textStatus);
			}
		};
		jQuery.ajax(params);
	},
	plusQuantity: function(){
		// Plus number quantiy product detail 
		if ( jQuery('input[name="quantity"]').val() != undefined ) {
			var currentVal = parseInt(jQuery('input[name="quantity"]').val());
			if (!isNaN(currentVal)) {
				jQuery('input[name="quantity"]').val(currentVal + 1);
			} else {
				jQuery('input[name="quantity"]').val(1);
			}
		}else {
			console.log('error: Not see elemnt ' + jQuery('input[name="quantity"]').val());
		}
	},
	minusQuantity: function(){
		// Minus number quantiy product detail 
		if ( jQuery('input[name="quantity"]').val() != undefined ) {
			var currentVal = parseInt(jQuery('input[name="quantity"]').val());
			if (!isNaN(currentVal) && currentVal > 1) {
				jQuery('input[name="quantity"]').val(currentVal - 1);
			}
		}else {
			console.log('error: Not see elemnt ' + jQuery('input[name="quantity"]').val());
		}
	},
	slug: function(str){
		// Change handleize
		str = str.toLowerCase();
		str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
		str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
		str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
		str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
		str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
		str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
		str = str.replace(/đ/g, "d");
		str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, "-");
		str = str.replace(/-+-/g, "-"); //thay thế 2- thành 1- 
		str = str.replace(/^\-+|\-+$/g, "");
		return str;
	},
	slug_words: function(str) {
		str = str.toLowerCase();
		str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
		str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
		str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
		str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
		str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
		str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
		str = str.replace(/đ/g, "d");
		str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, " ");
		str = str.replace(/-+-/g, ""); //thay thế 2- thành 1- 
		str = str.replace(/^\-+|\-+$/g, "");
		return str;
	},
	UpdateCartNoteToCheckout: function(form_id){
		var params = {
			type: 'POST',
			url: '/cart/update.js',
			data: jQuery('#' + form_id).serialize(),
			dataType: 'json',
			success: function(cart) {
				window.location = '/checkout';
			},
			error: function(XMLHttpRequest, textStatus) {
				Haravan.onError(XMLHttpRequest, textStatus);
			}
		};
		jQuery.ajax(params);
	}
}

jQuery(document).ready(function(){
	// Slider index
	if($('#slider-menu').length > 0 ) {
		$('#slider-menu .owl-carousel').owlCarousel({
			items: 1,
			dots: true,
			autoplay:true,
			autoplayTimeout:5000,
			autoplayHoverPause:true
		});
	}
	// Group blog owl-carousel index
	if ($('#blog-index').length > 0 ) {
		$('#blog-index.owl-carousel').owlCarousel({
			margin: 15,
			dots: false,
			responsive:{
				0:{
					items:1
				},
				768:{
					items:3
				},
				1024:{
					items:3
				},
				1200:{
					items:4
				}
			}
		});
	}
	// Group Brand owl-carousel index
	if ($('.brand-carousel').length > 0 ) {
		$('.brand-carousel .owl-carousel').owlCarousel({
			items: 10,
			margin: 15,
			dots: false,
			responsive:{
				0:{
					items:3
				},
				768:{
					items:6
				},
				1024:{
					items:8
				}
			}
		});
	}
	// Menu sidebarleft index
	if ($('#menusidebarleft').length > 0 ) {
		$('#menusidebarleft li a').click(function(e){
			if ( $(this).parent('li').attr('aria-expanded') == 'false' ) {
				e.preventDefault();
				if ( $(this).parent('.submenu-parent').length > 0 ) {
					$(this).parent('.submenu-parent').parent('ul').find('li.submenu-parent,li.submenu-children').attr('aria-expanded','false').children('ul').slideUp();

				} else {
					if ( $(this).parent('.submenu-children').length > 0 ) {
						$(this).parent('.submenu-children').parent('ul').children('li.submenu-parent,li.submenu-children').attr('aria-expanded','false').children('ul').slideUp();
					}
				}
				$(this).parent('li').attr('aria-expanded','true');
				$(this).nextAll(".dropdown-menu").slideDown();
				$(this).next('ul').slideDown();			
			}
		});
	}
	// Product related
	if ($('#product-related').length > 0 ) {
		$('#product-related .owl-carousel').owlCarousel ({
			margin: 15,
			nav: false,
			dots: false,
			responsive:{
				0:{
					items:2
				},
				768:{
					items:3
				},
				1024:{
					items:4
				}
			}
		});
		$('#product-related .owl-carousel').find('.owl-next').html("<svg class='svg-next-icon svg-next-icon-size-16' style='fill:#333'><use xlink:href='#icon-right-owlCarousel'></use></svg>");
		$('#product-related .owl-carousel').find('.owl-prev').html("<svg class='svg-next-icon svg-next-icon-size-16' style='fill:#333'><use xlink:href='#icon-left-owlCarousel'></use></svg>");
	}

	$(document).on("click",".dropdown-filter", function(){
		if ( $(this).parent().attr('aria-expanded') == 'false' ) {
			$(this).parent().attr('aria-expanded','true');
		} else {
			$(this).parent().attr('aria-expanded','false');
		}
	});
	$(document).on("click", "#trigger-mobile", function(){
		if ($('body').hasClass('responsive') == false) {
			$('body').addClass('responsive helper-overflow-hidden');
			$('html').addClass('helper-overflow-hidden');
			$(window).scrollTop(0);
			$('body').removeClass('hideresponsive');
			$("#box-wrapper").bind('click', function () {
				$("body").removeClass("responsive helper-overflow-hidden");
				$('html').removeClass('helper-overflow-hidden');
				$('body').addClass('hideresponsive');
				$("#box-wrapper").unbind('click');
			});
		}
		else {
			$("body").removeClass("responsive helper-overflow-hidden");
			$('html').removeClass('helper-overflow-hidden');
			$('body').addClass('hideresponsive');
		}
	});

	$(document).on("click", "#menu-mobile li a", function(){
		if ($(this).parent('li').hasClass('active')){
			$('#menu-mobile .menu-mobile ul').slideUp('fast');
			$(this).parent('li').removeClass('active');
		} else {
			$('#menu-mobile li.active').removeClass('active');
			$('#menu-mobile .menu-mobile ul').slideUp('fast');
			$(this).parent('li').addClass('active').find('ul').slideDown('fast');
		}
	});

	jQuery(document).on('click','.headtab-filter a',function(){
		$('.headtab-filter').removeClass('active');
		$(this).parent().addClass('active');
	});

	jQuery('.product-comments .tab-content > div').eq(0).addClass('active');

	$('#navbar li').hover(function(){
		$(this).addClass('open');
	}, function(){
		$(this).removeClass('open');
	});

	// Scroll Top
	jQuery(document).on("click", ".back-to-top", function(){
		jQuery(this).removeClass('display');
		jQuery('html, body').animate({
			scrollTop: 0			
		}, 600);
	});

	// Add attribute data-spy=scroll in element <a> when you click, it'll scroll to href="#abc"
	jQuery(document).on("click", "a[data-spy=scroll]", function(e) {
		e.preventDefault();
		jQuery('html, body').animate({scrollTop: (jQuery(jQuery(this).attr('href')).offset().top - 70) + 'px'}, 500);
	});

	// Active image thumb and change image featured ( product detail )
	jQuery(document).on("click", ".product-thumb a", function() {
		jQuery('.product-thumb').removeClass('active');
		jQuery(this).parents('.product-thumb').addClass('active');
		jQuery(".product-image-feature").attr("src",jQuery(this).attr("data-image"));
	});


	// Ajax get product in collection for page home
	if ( $('#tab-product-template').length > 0 ) {
		$('#tab-product-template').tabdrop({text: '<i class="fa fa-bars"></i>'});
	}

	// Scroll show icon section index
	jQuery(window).on("scroll", function() {	
		/* Process scroll header top  */		
		if ($(window).width() >= 768) {
			if( jQuery(window).scrollTop() > 0 ) {			
				jQuery('nav.navbar-main.navbar').addClass('affix-mobile');
			}	else {
				jQuery('nav.navbar-main.navbar').removeClass('affix-mobile');
			}
		}
		/* Scroll to top */
		if ( jQuery('.back-to-top').length > 0 && jQuery(window).scrollTop() > 500 ) {
			jQuery('.back-to-top').addClass('display');
		} else {
			jQuery('.back-to-top').removeClass('display');
		}
	});

});