Vue.component('v-select', VueSelect.VueSelect)

	new Vue({
	  	el: '#app',
	  	data: {
		  	user: "",
		  	country: 0,
		    users_options: [
		    	"Yogesh singh",
		    	"Sunil singh",
		    	"Sonarika bhadoria",
		    	"Akilesh sahu",
		    	"Mayank patidar"
		    ],
		    country_options: []
	  	},
	  	methods: {
	  		fetchOptions: function(search){
      			
      			var el = this;

      			// AJAX request
	  			axios.get('ajaxfile.php', {
	                params: {
						search: search,		
					}
	            })
	            .then(function (response) {
	               	
	               	// Update options
	                el.country_options = response.data; 
	                  
	            });

	  		},
	  		selectedOption: function(value){
	  			console.log("value : " + value);
	  		}
	  	}
	})