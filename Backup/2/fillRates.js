    // JScript File
    function fillBatch()
    {
        var BatchHeader = document.getElementById('txtBatchHeader');
        var TransFee = document.getElementById('txtTransFee');
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
        var DRQP = document.getElementById('txtDRQP');
        var obj = document.getElementById('DiscRateMidQualStep');
        var DRMQ = document.getElementById('txtDRMQ');
        var DRQNP = document.getElementById('txtDRQNP');
        var Interchange = document.getElementById('chkApplyInterchange');
        //function auto populates Disc Mid Qual
	    //If there is a Step specified in the database for this Processor 
	    //and Interchange is not Checked
	    if (( obj.value != "" ) && (Interchange.value == 0) )
	    {	        
	        if (( DRQP.disabled == false ) && (DRQP.value != ""))
	            DRMQ.value = parseFloat(DRQP.value) + parseFloat(obj.value);
	        else if (( DRQNP.disabled == false) && (DRQNP.value != ""))
	            DRMQ.value = parseFloat(DRQNP.value) + parseFloat(obj.value);
	        
	    }	    
    }
    
    function fillNonQual()
    {
        var DRQP = document.getElementById('txtDRQP');
        var obj = document.getElementById('DiscRateNonQualStep');
        var DRNQ = document.getElementById('txtDRNQ');
        var DRQNP = document.getElementById('txtDRQNP');
        if ( obj.value != "")
        {
            if (( DRQP.disabled == false) && (DRQP.value != ""))
                DRNQ.value = parseFloat(DRQP.value) + parseFloat(obj.value);
            else if ((DRQNP.disabled == false) && (DRQNP.value != ""))
                DRNQ.value = parseFloat(DRQNP.value) + parseFloat(obj.value);
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

