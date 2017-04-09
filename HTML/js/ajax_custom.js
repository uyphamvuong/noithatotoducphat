$(document).ready(function() {
	
	$('.click_showsp').click(function(event) {
		$('.click_showsp').removeClass('active');
		var id = $(this).data('id');
		$("#loader").fadeIn(3000);
		$("#hien_sp").css('display', 'none');
		$(this).addClass('active');
		$.ajax({
			url: './ajax/load_sp.php',
			type: 'POST',
			data: {id: id},
			success: function(msg){
				$("#loader").fadeOut(1000, function() {
					$("#hien_sp").fadeIn(1000).html(msg);
				}); 
			}
		})
	});
});