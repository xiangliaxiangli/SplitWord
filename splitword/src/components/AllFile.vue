<template>
  <el-main>
    <v-container>
      <el-card>
        <div class="tag-group">
          <h2>分类</h2>
          <el-tag
              v-for="field in fields"
              :key="field.name"
              :type="field.type"
              effect="dark"
              @click="choose(field)">
            {{field.name}}
          </el-tag>
        </div>
      </el-card>
      <br/>
      <el-carousel :interval="4000" type="card" height="300px">
        <el-carousel-item height="800px" v-for="item in 6" :key="item">
          <el-image :src="src"></el-image>
        </el-carousel-item>
      </el-carousel>
    </v-container>
  </el-main>
</template>

<script>
export default {
  name: 'AllFile',
  data() {
    return {
      fields: [
        {name:"计算机",type:"info"},
        {name:"数学",type:"info"},
        {name:"医学",type:"info"},
        {name:"财经",type:"info"},
        {name:"物理",type:"info"},
        {name:"化学",type:"info"}
      ],
      src: 'https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg'
    }
  },methods:{
    //选择感兴趣的领域
    choose(tag){
      console.log(tag)
      //先将所有的都变灰
      for(let i = 0;i<this.fields.length;i++){
        this.fields[i].type = 'info';
      }
      //将目标选中
      tag.type = "success";
      this.$router.push({
        name: 'ShowFiles',
        params:{
          Field: tag.name
        }
      })
    }
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
.el-carousel__item h3 {
  color: #475669;
  font-size: 14px;
  opacity: 0.75;
  line-height: 500px;
  margin: 0;
}
.el-carousel__item:nth-child(2n) {
  background-color: #99a9bf;
  line-height: 500px;
}

.el-carousel__item:nth-child(2n+1) {
  background-color: #d3dce6;
}
</style>