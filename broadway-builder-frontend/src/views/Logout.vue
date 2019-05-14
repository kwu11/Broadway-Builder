<script>
import axios from "axios";
import User from '@/User.js';
export default {
    methods:{
        async Logout(){
            await axios
            .post("https://api.broadwaybuilder.xyz/user/applogout",undefined,{
                headers:{
                    'Authorization':'Bearer ${User.token}'
                }
            })
            .then(
                response =>{
                    if(response.status==200){
                        window.localStorage.removeItem('token');
                    }
                    this.$router.push({name:'home'});
                }
            )
        }
    },
    created() {
        this.Logout();
    }
}
</script>
