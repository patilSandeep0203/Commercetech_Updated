    // JScript File
    function fillBatch()
    {
        var BatchHeader = document.getElementById('txtBatchHeader');
        var TransFee = document.getElementById('txtTransFee');
        var Processor = document.getElementById('lstProcessorNames');
        if (Processor == 'Sage Payment Solutions')
            BatchHeader.value = 0.00; 
        else
            BatchHeader.value = TransFee.value;
    }
    
    function fillNBCFee()
    {
        var NBC = document.getElementById('txtNBCTransFee');
        var TransFee = document.getElementById('txtTransFee');
        NBC.value = TransFee.value;
    }
       
    function fillMidQual()
    {
        //function auto populates Disc Mid Qual
        var obj = document.getElementById('DiscRateMidQualStep');
        var DRMQ = document.getElementById('txtDRMQ');
        var CardPresent = document.getElementById('rdbCP');
        var Interchange = document.getElementById('chkApplyInterchange');
        if (CardPresent.checked)
            var DR = document.getElementById('txtDRQP');
        else
            var DR = document.getElementById('txtDRQNP');            	
            
        //if Interchange option is available
        if (Interchange != null)
        {
    	     //if Interchange is checked
	        if (Interchange.checked)  
	             DRMQ.value = DR.value;       
	        //If there is a Step specified in the database for this Processor
	        else if ( obj.value != "" ) 	        
	             DRMQ.value = parseFloat(DR.value) + parseFloat(obj.value)  	        
	    }  
	   	    
    }
    
    function fillNonQual()
    {
        var obj = document.getElementById('DiscRateNonQualStep');
        var CardPresent = document.getElementById('rdbCP');
        var Interchange = document.getElementById('chkApplyInterchange');
        var DRNQ = document.getElementById('txtDRNQ');
        
        if (CardPresent.checked)
            var DR = document.getElementById('txtDRQP');
        else
            var DR = document.getElementById('txtDRQNP');
          
         //if Interchange option is available
        if (Interchange != null)
        {	     
            if (Interchange.checked)  
	            DRNQ.value = DR.value;
	        else if ( obj.value != "" ) 
                DRNQ.value = parseFloat(DR.value) + parseFloat(obj.value);
        }

    }
    
    function fillDebit()
    {
        var DRQP = document.getElementById('txtDRQP');
        var DRQD = document.getElementById('txtDRQD');
        var DRQNP = document.getElementById('txtDRQNP');
        if ( DRQP.disabled == false)
            DRQD.value = DRQP.value;
        else if ( DRQNP.disabled == false )
                DRQD.value = DRQNP.value;
    }

