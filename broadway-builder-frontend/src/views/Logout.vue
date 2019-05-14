<script>
import axios from "axios";
export default {
    methods:{
        async Logout(){
            await axios
            .post("https://api.broadwaybuilder.xyz/user/applogout",undefined,{
                headers:{
                    Authorization: `Bearer ${this.$store.state.token}`
                }
            })
            .then(async () => {
                window.localStorage.removeItem('token');
                await this.$store.dispatch('updateUserInfo');
                this.$router.push({name:'home'});
            })
            .catch(async () => {
                window.localStorage.removeItem('token');
                await this.$store.dispatch('updateUserInfo');
                this.$router.push({name:'home'});
            });
        }
    },
    created() {
        this.Logout();
    }
}
</script>
