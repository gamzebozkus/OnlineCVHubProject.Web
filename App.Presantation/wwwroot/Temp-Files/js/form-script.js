/* global $, alert, console */
$(document).ready(function(){
    'use strict';
    /*
    var x = 0,
        inptSkill = $('#input-skills');
    
    inptSkill.on('keyup',function(e){
        e.stopPropagation();
        if(e.which == 13){
            e.preventDefault();
            
            var curVal = inptSkill.val().trim();
            var found = false;
            for(var i = 0; i<x; i++){
                var str = $('.tag-span').eq(i).text().trim();
                if(str == curVal){
                    found=true;
                }
            }
            if(!found && curVal.length > 1){
                $('.tags').append('<span class="tag-span"><i class="fa fa-times"></i> '+curVal + '<input hidden type="text" value="'+curVal+'" name="skills[]"></span>');
                x++;
            }

            inptSkill.val('');
            
        }
    });
    */
    //$('.fa-plus-circle').click(function(){
    //    var curVal = inptSkill.val().trim();
    //    var found = false;
    //    for(var i = 0; i<x; i++){
    //        var str = $('.tag-span').eq(i).text().trim();
    //        if(str == curVal){
    //            found=true;
    //        }
    //    }
    //    if(!found && curVal.length > 1){
    //        $('.tags').append('<span class="tag-span"><i class="fa fa-times"></i> '+curVal + '<input hidden type="text" value="'+curVal+'" name="skills[]"></span>');
    //        x++;
    //    }
        
    //    inptSkill.val('');
    //});
    
    // Remove Tag On Click
    
    //$('.tags').on('click','.tag-span i',function(){
    //    $(this).parent('.tag-span').fadeOut(500).remove();
    //    x--;
        
    //});
    
    // Add education block
    
    //$('#add-edu').on('click',function(){
    //    $('.all-edus').append('<div class="add-border"><span></span><h2>Eðitim Ekle</h2><span></span></div><div class="new-edu"><label>Field of study:</label>                  <input type="text" name="EducationInfos[@i].Major" id="bolumInput" onkeyup="updateCV('bolum', this.value)" class="form-control" placeholder="Örnek:Bilgisayar Mühendisliði">          <label>Degree:</label> <input type="text" name="EducationInfos[@i].Degree" id="dereceInput" onkeyup="updateCV('derece', this.value)" class="form-control" placeholder="Derece: Lisans" ><label>School:</label>  <input type="text" name="EducationInfos[@i].School" id="okulInput" onkeyup="updateCV('okul', this.value)" class="form-control" placeholder="" ><div class="form-row"><div class="col">                   <label>From year:</label>  <input type="month" name="EducationInfos[@i].StartDate" id="baslamaYiliInput" onkeyup="updateCV('baslamaYili', this.value)" class="form-control" >              </div><div class="col"><label>To year (optional=present):</label>                                 <input type="month" name="EducationInfos[@i].EndDate" id="bitisYiliInput" onkeyup="updateCV('bitisYili', this.value)" class="form-control" ></div></div></div>'); 
    //});
    
    
    // Add Experience block
    
    $('#add-exp').on('click',function(){
       $('.all-exps').append('<div class="add-border"><span></span><h2>Deneyim Ekle</h2><span></span></div><div class="new-exp"><label>Title:</label><input type="text" name="exp[]" class="form-control" placeholder="Ex: Web Developer"><label>Company:</label>                      <input type="text" name="exp[]" class="form-control" placeholder="Ex: ProgressSoft">             <div class="form-row"><div class="col"><label>From year:</label>                                 <input type="month" name="exp[]" class="form-control"></div><div class="col">                     <label>To year (optional=present):</label><input type="month" name="exp[]" class="form-control">  </div></div><label>Description (optional):</label><textarea name="exp[]" class="form-control"></textarea></div>');
    });
    
    // Add skills block
    
    //$('.add-skills').on('click',function(){
    //    $('.all-skills').append('<div class="add-border"><span></span><h2>Beceri Ekle</h2><span></span></div><div class="new-skills"><label>Skill</label> <input type="text" name="skill" class="form-control">          <label>Proficiency</label><input type="text" name="skills[]" class="form-control"></div>');
        
    //});

    // Add socials block
    
    $('.add-socials').on('click',function(){
        $('.all-socials').append('<div class="add-border"><span></span><h2>New social</h2><span></span></div><div class="new-socials"><label>Social Name</label> <input type="text" name="social" class="form-control">          <label>Social Link</label><input type="text" name="socials[]" class="form-control"> <label>Social icon image (16px*16px)</label><input type="file" name="socials[]" class="form-control" /></div>');
        
    });

    // Add socials block
    
    //$('.add-hoppies').on('click',function(){
    //    $('.all-hoppies').append('<div class="add-border"><span></span><h2>Hobi Ekle</h2><span></span></div><div class="new-hoppies"> <label>Hoppy icon image (32px*32px)</label><input type="file" name="hoppies[]" class="form-control" /></div>');
        
    //});
    
    
});