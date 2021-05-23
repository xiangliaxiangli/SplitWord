<template>
  <el-main>
    <v-container>
      <div v-for="(file,index) in files">
        <el-card

        >
          <div>
            <el-link
                @click="showFile(file.fileID)"
            >{{file.filename}}</el-link>
          </div>
          <br/>
          <div>
            <el-tag
                v-for="keyword in files[index].keywords"
                :key="keyword"
                type="success"
                effect="dark">
              {{keyword}}
            </el-tag>
          </div>
        </el-card>
        <br/>
      </div>
    </v-container>
  </el-main>
</template>

<script>
export default {
  name: "ShowFiles",
  data(){
    return{
      files:[
        {
          filename: '',
          fileID: null,  //方便用户点击文章链接的时候进行查找
          keywords:[

          ]
        }
      ],
    }
  },
  methods:{
    showFilesByField(){
      let Field = this.$route.params.Field;
      this.axios.post('https://localhost:44366/api/Hello/getAllFilesByField?Field='+Field).then(response=>{
        console.log(response.data)
        this.files = response.data
      })
    },
    //跳转显示文章内容
    showFile(fileID){
      console.log("你好")
      this.$router.push({
        name: 'Field_File',
        params:{
          FileID: fileID
        }
      })
    }
  },
  created() {
    this.showFilesByField();
  }
}
</script>

<style scoped>
.el-tag + .el-tag {
  margin-left: 10px;
}
.button-new-tag {
  margin-left: 10px;
  height: 32px;
  line-height: 30px;
  padding-top: 0;
  padding-bottom: 0;
}
.input-new-tag {
  width: 90px;
  margin-left: 10px;
  vertical-align: bottom;
}
</style>