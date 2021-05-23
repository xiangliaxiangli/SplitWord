<template>
  <el-main
      v-loading="loading"
      element-loading-text="拼命加载中"
      element-loading-spinner="el-icon-loading"
      element-loading-background="rgba(0, 0, 0, 0.8)"
      style="width: 100%;height: 100%"
  >
    <el-card :hidden=hidden>
      <h2  class="green lighten-3 black--text">{{article.filename}}</h2>
    </el-card>
    <br/>
    <el-card :hidden=hidden>
      <h5 class="blue lighten-3 black--text">{{article.content}}</h5>
    </el-card>
    <br/>
    <el-card>
      <a :href=url :hidden=hidden>下载文件</a>
    </el-card>
  </el-main>
</template>

<script>
export default {
  name: "Field_File",
  data(){
    return {
      url: "https://localhost:44366/api/Hello/DownloadFileByID/",
      article:{
        filename:null,
        content:null
      },
      loading: true,
      hidden: true
    }
  },
  methods:{
    //向后台要求请求文件内容
    show_file(FileID){
      this.axios.get('https://localhost:44366/api/Hello/getFileByID/'+FileID).then(resp=>{
        console.log(resp.data)
        this.article.filename = resp.data["FileName"];
        this.article.content = resp.data["Content"];
        this.loading = false; //加载结束
        this.hidden = false;
      })
    }
  },
  created() {
    this.url =this.url+this.$route.params.FileID;
    console.log(this.url);
    this.show_file(this.$route.params.FileID);
  }
}
</script>

<style scoped>

</style>