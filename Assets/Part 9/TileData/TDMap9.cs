public class TDMap9 {

	protected TDTile9[] tiles;

	int width, height;

	public TDMap9(int width, int height) {
		this.width = width;
		this.height = height;
		this.tiles = new TDTile9[width * height];
	}

	public TDTile9 GetTile(int x, int y){
		if(x<0 || x>=this.width || y<0 || y>=this.height){
			return null;
		}
		return this.tiles[y*width+x];
	}

}
