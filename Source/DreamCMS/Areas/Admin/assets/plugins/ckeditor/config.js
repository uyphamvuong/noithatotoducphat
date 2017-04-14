/**
 * @license Copyright (c) 2003-2014, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
	config.filebrowserBrowseUrl = '/Areas/Admin/assets/plugins/ckfinder/ckfinder32489.html';
	config.filebrowserImageBrowseUrl = '/Areas/Admin/assets/plugins/ckfinder/ckfinder32489.html?type=Images';
	config.filebrowserFlashBrowseUrl = '/Areas/Admin/assets/plugins/ckfinder/ckfinder32489.html?type=Flash';
	config.filebrowserUploadUrl = '/Areas/Admin/assets/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
	config.filebrowserImageUploadUrl = '/Areas/Admin/assets/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
	config.filebrowserFlashUploadUrl = '/Areas/Admin/assets/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';
};